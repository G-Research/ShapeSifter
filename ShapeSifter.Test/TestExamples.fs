namespace ShapeSifter.Test

open NUnit.Framework
open HCollections
open ShapeSifter
open ShapeSifter.Patterns
open TypeEquality
open FsUnitTyped

[<TestFixture>]
module TestExamples =

    [<Test>]
    let ``Simple example`` () =
        let tryString (a : 'a) : string option =
            match tType<'a> with
            | String (teq : Teq<'a, string>) -> Teq.castTo teq a |> Some
            | _ -> None

        tryString 1234 |> shouldEqual None
        tryString "hello" |> shouldEqual (Some "hello")

    [<Test>]
    let ``List example 1`` () =
        let tryListLength (a : 'a) : int option =
            match tType<'a> with
            | List crate ->
                { new ListTeqEvaluator<_, _> with
                    member _.Eval (teq : Teq<'a, 'b list>) =
                        a |> Teq.castTo teq |> List.length |> Some
                }
                |> crate.Apply
            | _ -> None

        tryListLength "hello" |> shouldEqual None
        tryListLength [ 'a' .. 'z' ] |> shouldEqual (Some 26)

    [<Test>]
    let ``List example 2`` () =
        let tryListSomeCount (a : 'a) : int option =
            match tType<'a> with
            | List crate ->
                { new ListTeqEvaluator<_, _> with
                    member _.Eval (teq1 : Teq<'a, 'b list>) =
                        match tType<'b> with
                        | Option crate ->
                            { new OptionTeqEvaluator<_, _> with
                                member _.Eval (teq2 : Teq<'b, 'c option>) =
                                    let teq : Teq<'a, 'c option list> = Teq.transitivity teq1 (Teq.Cong.list teq2)

                                    let xs : 'c option list = Teq.castTo teq a

                                    xs |> List.filter Option.isSome |> List.length |> Some
                            }
                            |> crate.Apply
                        | _ -> None
                }
                |> crate.Apply
            | _ -> None

        tryListSomeCount [ None ; Some 'a' ; None ; Some 'b' ; Some 'c' ]
        |> shouldEqual (Some 3)

    [<Test>]
    let ``Tuple example 1`` () =
        let tryTupleLength (a : 'a) : int option =
            match tType<'a> with
            | Tuple crate ->
                { new TupleConvEvaluator<_, _> with
                    member _.Eval (ts : 'ts TypeList) (conv : Conv<'a, 'ts HList>) =
                        a |> conv.To |> HList.length |> Some
                }
                |> crate.Apply
            | _ -> None

        tryTupleLength ("hello", false) |> shouldEqual (Some 2)
        tryTupleLength (5, 5, 5, 5) |> shouldEqual (Some 4)

    [<Test>]
    let ``Tuple example 2`` () =
        let trySumTupleInts (a : 'a) : int option =
            match tType<'a> with
            | Tuple crate ->
                { new TupleConvEvaluator<_, _> with
                    member _.Eval _ (conv : Conv<'a, 'ts HList>) =
                        let xs : 'ts HList = a |> conv.To

                        let folder =
                            { new HListFolder<int> with
                                member _.Folder sum (x : 'b) =
                                    match tType<'b> with
                                    | Int teq -> sum + (x |> Teq.castTo teq)
                                    | _ -> sum
                            }

                        HList.fold folder 0 xs |> Some
                }
                |> crate.Apply
            | _ -> None

        trySumTupleInts (5, 5, 5, 5) |> shouldEqual (Some 20)
        trySumTupleInts (5, false, 3, "hello") |> shouldEqual (Some 8)
        trySumTupleInts ("hello", false) |> shouldEqual (Some 0)

    let rec shoutify<'ts> (xs : 'ts HList) : 'ts HList =
        match xs |> HList.toTypeList |> TypeList.split with
        | Choice1Of2 _ -> xs
        | Choice2Of2 crate ->

        { new TypeListConsEvaluator<_, _> with
            member _.Eval _ (teq : Teq<'ts, 'u -> 'us>) =
                let xs : ('u -> 'us) HList = xs |> Teq.castTo (HList.cong teq)

                let head =
                    match tType<'u> with
                    | String teq -> (xs |> HList.head |> Teq.castTo teq).ToUpper () |> Teq.castFrom teq
                    | _ -> xs |> HList.head

                let tail = xs |> HList.tail |> shoutify

                HList.cons head tail |> Teq.castFrom (HList.cong teq)
        }
        |> crate.Apply

    let tryShoutifyRecord (a : 'a) : 'a option =
        match tType<'a> with
        | Record crate ->
            { new RecordConvEvaluator<_, _> with
                member _.Eval _ _ (conv : Conv<'a, 'ts HList>) =
                    let xs : 'ts HList = a |> conv.To
                    shoutify xs |> conv.From |> Some
            }
            |> crate.Apply
        | _ -> None

    type MyRecord =
        {
            FirstName : string
            LastName : string
            Age : int
            Location : string
        }

    [<Test>]
    let ``Record example`` () =
        let sample =
            {
                FirstName = "Bob"
                LastName = "Sample"
                Age = 35
                Location = "London"
            }

        let expected =
            {
                FirstName = "BOB"
                LastName = "SAMPLE"
                Age = 35
                Location = "LONDON"
            }

        tryShoutifyRecord sample |> shouldEqual (Some expected)
