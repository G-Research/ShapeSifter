// Note: run 'dotnet publish' in the root of this repo to ensure that the dlls referenced in this script are present

#r "netstandard"
#I @"..\ShapeSifter\bin\Debug\netstandard2.0\publish"
#r "TypeEquality.dll"
#r "HCollections.dll"
#r "ShapeSifter.dll"

open HCollections
open TeqCrate
open TeqCrate.Patterns
open TypeEquality



// Simple Example

let tryString (a : 'a) : string option =
    match tType<'a> with
    | String (teq : Teq<'a, string>) -> Teq.castTo teq a |> Some
    | _ -> None

tryString 1234
tryString "hello"



// List Example

let tryListLength (a : 'a) : int option =
    match tType<'a> with
    | List crate ->
        crate.Apply
            { new ListTeqEvaluator<_, _> with
                member __.Eval (teq : Teq<'a, 'b list>) =
                    a |> Teq.castTo teq |> List.length |> Some
            }
    | _ -> None

tryListLength "hello"
tryListLength [ 'a' .. 'z' ]



// List Example 2

let tryListSomeCount (a : 'a) : int option =
    match tType<'a> with
    | List crate ->
        crate.Apply
            { new ListTeqEvaluator<_, _> with
                member __.Eval (teq1 : Teq<'a, 'b list>) =
                    match tType<'b> with
                    | Option crate ->
                        crate.Apply
                            { new OptionTeqEvaluator<_, _> with
                                member __.Eval (teq2 : Teq<'b, 'c option>) =
                                    let teq : Teq<'a, 'c option list> = Teq.transitivity teq1 (Teq.Cong.list teq2)
                                    let xs : 'c option list = Teq.castTo teq a

                                    xs |> List.filter Option.isSome |> List.length |> Some
                            }
                    | _ -> None
            }
    | _ -> None

tryListSomeCount [ None ; Some 'a' ; None ; Some 'b' ; Some 'c' ]



// Tuple Example

let tryTupleLength (a : 'a) : int option =
    match tType<'a> with
    | Tuple crate ->
        crate.Apply
            { new TupleConvEvaluator<_, _> with
                member __.Eval (ts : 'ts TypeList) (conv : Conv<'a, 'ts HList>) = a |> conv.To |> HList.length |> Some
            }
    | _ -> None

tryTupleLength ("hello", false)
tryTupleLength (5, 5, 5, 5)



// Tuple Example 2

let trySumTupleInts (a : 'a) : int option =
    match tType<'a> with
    | Tuple crate ->
        crate.Apply
            { new TupleConvEvaluator<_, _> with
                member __.Eval _ (conv : Conv<'a, 'ts HList>) =
                    let xs : 'ts HList = a |> conv.To

                    let folder =
                        { new HListFolder<int> with
                            member __.Folder sum (x : 'b) =
                                match tType<'b> with
                                | Int teq -> sum + (x |> Teq.castTo teq)
                                | _ -> sum
                        }

                    HList.fold folder 0 xs |> Some
            }
    | _ -> None

trySumTupleInts (5, 5, 5, 5)
trySumTupleInts (5, false, 3, "hello")
trySumTupleInts ("hello", false)


// Record Example

let rec shoutify<'ts> (xs : 'ts HList) : 'ts HList =
    match xs |> HList.toTypeList |> TypeList.split with
    | Choice1Of2 _ -> xs
    | Choice2Of2 crate ->

    crate.Apply
        { new TypeListConsEvaluator<_, _> with
            member __.Eval _ (teq : Teq<'ts, 'u -> 'us>) =
                let xs : ('u -> 'us) HList = xs |> Teq.castTo (HList.cong teq)

                let head =
                    match tType<'u> with
                    | String teq -> (xs |> HList.head |> Teq.castTo teq).ToUpper () |> Teq.castFrom teq
                    | _ -> xs |> HList.head

                let tail = xs |> HList.tail |> shoutify

                HList.cons head tail |> Teq.castFrom (HList.cong teq)
        }

let tryShoutifyRecord (a : 'a) : 'a option =
    match tType<'a> with
    | Record crate ->
        crate.Apply
            { new RecordConvEvaluator<_, _> with
                member __.Eval _ _ (conv : Conv<'a, 'ts HList>) =
                    let xs : 'ts HList = a |> conv.To
                    shoutify xs |> conv.From |> Some
            }
    | _ -> None

type MyRecord =
    {
        FirstName : string
        LastName : string
        Age : int
        Location : string
    }

let sample =
    {
        FirstName = "Bob"
        LastName = "Sample"
        Age = 35
        Location = "London"
    }

tryShoutifyRecord sample
