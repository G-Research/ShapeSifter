// Note: run 'dotnet publish' in the root of this repo to ensure that the dlls referenced in this script are present

#r "netstandard"
#I @"..\TeqCrate\bin\Debug\netstandard2.0\publish"
#r "TypeEquality.dll"
#r "HCollections.dll"
#r "TeqCrate.dll"

open HCollections
open System
open System.IO
open TeqCrate
open TeqCrate.Patterns
open TypeEquality

let parseCell<'a> : string -> 'a =
    match tType<'a> with
    | String (teq : Teq<'a, string>) -> Teq.castFrom teq
    | Bool (teq : Teq<'a, bool>) -> Boolean.Parse >> Teq.castFrom teq
    | Int (teq : Teq<'a, int>) -> Int32.Parse >> Teq.castFrom teq
    | Float (teq : Teq<'a, float>) -> Double.Parse >> Teq.castFrom teq
    | DateTime (teq : Teq<'a, DateTime>) -> DateTime.Parse >> Teq.castFrom teq
    | _ -> failwithf "Error - the type %s is not supported" (typeof<'a>.FullName)

let rec parseRow<'ts> (ts : 'ts TypeList) (cells : string list) : 'ts HList =
    match TypeList.split ts with
    | Choice1Of2 (teq : Teq<'ts, unit>) -> HList.empty |> Teq.castFrom (HList.cong teq)
    | Choice2Of2 crate ->

    crate.Apply
        { new TypeListConsEvaluator<_, _> with
            member __.Eval (us : 'us TypeList) (teq : Teq<'ts, 'u -> 'us>) =
                let head = cells |> List.head |> parseCell<'u>
                let tail = cells |> List.tail |> parseRow us

                HList.cons head tail |> Teq.castFrom (HList.cong teq)
        }

let tryParse<'record> (fileInfo : FileInfo) : 'record seq option =
    match tType<'record> with
    | Record crate ->
        crate.Apply
            { new RecordConvEvaluator<_, _> with
                member __.Eval _ (ts : 'ts TypeList) (conv : Conv<'record, 'ts HList>) =
                    File.ReadLines fileInfo.FullName
                    |> Seq.map (fun row -> row.Split ',' |> List.ofArray |> parseRow ts |> conv.From)
                    |> Some
            }
    | _ -> None
