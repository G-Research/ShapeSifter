namespace ShapeSifter.Test

open HCollections
open NUnit.Framework
open FsUnitTyped
open ShapeSifter
open ShapeSifter.Patterns
open TypeEquality

[<TestFixture>]
module TestPatterns =

    [<Test>]
    let ``Teq active pattern distinguishes int from string`` () =
        let t1 = tType<int>
        let t2 = tType<string>

        match t1 with
        | Teq t2 _teq -> failwith "Expected int != string"
        | _ -> ()

    [<Test>]
    let ``Option active pattern recognises an option`` () : unit =
        let print (x : 'a) : string option =
            match tType<'a> with
            | Option crate ->
                { new OptionTeqEvaluator<_, _> with
                    member _.Eval<'b> (teq : Teq<'a, 'b option>) =
                        Teq.castTo teq x |> Option.map (sprintf "%O")
                }
                |> crate.Apply
            | _ -> failwith "should have received an option"

        print (Some 3) |> shouldEqual (Some "3")
        print None |> shouldEqual None

    [<Test>]
    let ``Choice2 active pattern recognises a choice`` () : unit =
        let print (x : 'a) : Choice<string, string> =
            match tType<'a> with
            | Choice2 crate ->
                { new Choice2TeqEvaluator<_, _> with
                    member _.Eval teq =
                        match Teq.castTo teq x with
                        | Choice1Of2 c1 -> c1.ToString () |> Choice1Of2
                        | Choice2Of2 c2 -> c2.ToString () |> Choice2Of2
                }
                |> crate.Apply
            | _ -> failwith "should have received a choice2"

        print (Choice1Of2 3) |> shouldEqual (Choice1Of2 "3")
        print (Choice2Of2 "hi") |> shouldEqual (Choice2Of2 "hi")

    let tryGetArrayLength (arr : 'a) : int option =
        match tType<'a> with
        | Array c ->
            { new ArrayTeqEvaluator<_, _> with
                member _.Eval teq = (Teq.castTo teq arr).Length |> Some
            }
            |> c.Apply
        | _ -> None

    [<Test>]
    let ``Array active pattern recognises an array`` () =
        let arr = [| "foo" ; "bar" |]
        tryGetArrayLength arr |> shouldEqual (Some 2)

    let tryGetListLength (xs : 'a) : int option =
        match tType<'a> with
        | List c ->
            c.Apply
                { new ListTeqEvaluator<_, _> with
                    member _.Eval teq =
                        xs |> Teq.castTo teq |> List.length |> Some
                }
        | _ -> None

    [<Test>]
    let ``List active pattern recognises a list`` () =
        let xs = [ 1..10 ]
        tryGetListLength xs |> shouldEqual (Some 10)

    let tryGetMapCount (map : 'a) : int option =
        match tType<'a> with
        | Map c ->
            { new MapTeqEvaluator<_, _> with
                member _.Eval teq =
                    map |> Teq.castTo teq |> Map.count |> Some
            }
            |> c.Apply
        | _ -> None

    [<Test>]
    let ``Map active pattern recognises a map`` () =
        let map = Map.empty |> Map.add "foo" 3 |> Map.add "bar" 12
        tryGetMapCount map |> shouldEqual (Some 2)

    [<Test>]
    let ``Tuple active pattern recognises a tuple`` () =
        let tuple = 5, "hello", false, 8, 2
        let sumOfInts = Tuple.tryFoldTuple (HListFolder.makeElementFolder (+)) 0 tuple
        sumOfInts |> shouldEqual (Some 15)

    [<Test>]
    let ``Fun active pattern recognises a function`` () =
        match tType<int -> string> with
        | Fun c ->
            let dom, ran =
                { new FunTeqEvaluator<_, _> with
                    member _.Eval (teq : Teq<int -> string, 'a -> 'b>) = typeof<'a>, typeof<'b>
                }
                |> c.Apply

            dom |> shouldEqual typeof<int>
            ran |> shouldEqual typeof<string>

        | _ -> failwith "expected a function type"

    [<Test>]
    let ``Pair active pattern recognises a pair`` () =
        match tType<int * string> with
        | Pair c ->
            let t1, t2 =
                { new PairTeqEvaluator<_, _> with
                    member _.Eval (teq : Teq<int * string, 'a * 'b>) = typeof<'a>, typeof<'b>
                }
                |> c.Apply

            t1 |> shouldEqual typeof<int>
            t2 |> shouldEqual typeof<string>

        | _ -> failwith "expected a pair type"

    [<Test>]
    let ``Unit active pattern recognises a unit`` () =
        // This test is useful because there are certain aspects of F# which behave oddly
        // in the presence of unit. (It turns out that units are just fine here.)

        match tType<unit> with
        | Unit teq -> Teq.castFrom teq () |> shouldEqual ()

        | _ -> failwith "expected a unit type"

    [<Test>]
    let ``Triple active pattern recognises a triple`` () =

        match tType<int * string * bool> with
        | Triple c ->
            let t1, t2, t3 =
                { new TripleTeqEvaluator<_, _> with
                    member _.Eval (teq : Teq<int * string * bool, 'a * 'b * 'c>) = typeof<'a>, typeof<'b>, typeof<'c>
                }
                |> c.Apply

            t1 |> shouldEqual typeof<int>
            t2 |> shouldEqual typeof<string>
            t3 |> shouldEqual typeof<bool>

        | _ -> failwith "expected a triple"

    type TestRecord =
        {
            Foo : string
            Bar : int
            Baz : string
        }

    let tryGetStringKeyValues (record : 'a) : Map<string, string> option =
        match tType<'a> with
        | Record c ->
            { new RecordConvEvaluator<_, _> with
                member _.Eval names _ conv =
                    let folder =
                        let f (names : string list, map) (value : string option) =
                            let map =
                                match value with
                                | Some v -> Map.add (names |> List.head) v map
                                | None -> map

                            names |> List.tail, map

                        HListFolder.makeGappedElementFolder f

                    let names = names |> List.map TypeField.name

                    record |> conv.To |> HList.fold folder (names, Map.empty) |> snd
            }
            |> c.Apply
            |> Some
        | _ -> None

    [<Test>]
    let ``Record active pattern recognises a record`` () =
        let r =
            {
                Foo = "hello"
                Bar = 1234
                Baz = "world"
            }

        let pairs =
            tryGetStringKeyValues r |> Option.map (Map.toSeq >> Seq.sort >> List.ofSeq)

        let expected = Some [ "Baz", "world" ; "Foo", "hello" ]

        pairs |> shouldEqual expected

    type TestUnion =
        | Foo
        | Bar of int * string * bool
        | Baz of string * float
        | Quux of string

    [<Test>]
    let ``Union active pattern recognises a union`` () =
        let testValue = Bar (1234, "test", true)

        let result =
            match tType<TestUnion> with
            | Union c ->
                { new UnionConvEvaluator<_, _> with
                    member _.Eval names ts (conv : Conv<TestUnion, 'a HUnion>) =

                        let expectedNames = [ "Foo" ; "Bar" ; "Baz" ; "Quux" ]
                        let actualNames = names |> List.map TypeField.name
                        actualNames |> shouldEqual expectedNames

                        let expectedUnionType =
                            tType<(unit -> int * string * bool -> string * float -> string -> unit) HUnion>

                        match tType<'a HUnion> with
                        | Teq expectedUnionType teq ->
                            let converted = testValue |> conv.To |> Teq.castTo teq

                            match HUnion.split converted with
                            | Choice1Of2 _ -> failwith "expected Choice2Of2"
                            | Choice2Of2 union ->
                                match HUnion.split union with
                                | Choice1Of2 (_ : int, _ : string, _ : bool) ->
                                    let _convertedBack = converted |> Teq.castFrom teq |> conv.From
                                    true
                                | Choice2Of2 _ -> failwith "expected Choice1Of2"
                        | _ -> failwith "expected Teq"
                }
                |> c.Apply
            | _ -> failwith "expected Union"

        result |> shouldEqual true

    [<Test>]
    let ``SumOfProducts active pattern recognises a union`` () =
        let testValue = Bar (1234, "test", true)

        let result =
            match tType<TestUnion> with
            | SumOfProducts c ->
                { new SumOfProductsConvEvaluator<_, _> with
                    member _.Eval names ts (conv : Conv<TestUnion, 'a SumOfProducts>) =

                        let expectedNames = [ "Foo" ; "Bar" ; "Baz" ; "Quux" ]
                        names |> shouldEqual expectedNames

                        let expectedUnionType =
                            tType<
                                (unit
                                    -> (int -> string -> bool -> unit)
                                    -> (string -> float -> unit)
                                    -> (string -> unit)
                                    -> unit) SumOfProducts
                             >

                        match tType<'a SumOfProducts> with
                        | Teq expectedUnionType teq ->
                            let converted = testValue |> conv.To |> Teq.castTo teq

                            match SumOfProducts.split converted with
                            | Choice1Of2 _ -> failwith "expected Choice2Of2"
                            | Choice2Of2 sop ->
                                match SumOfProducts.split sop with
                                | Choice1Of2 (_ : (int -> string -> bool -> unit) HList) ->
                                    let _convertedBack = converted |> Teq.castFrom teq |> conv.From
                                    true
                                | Choice2Of2 _ -> failwith "expected Choice1Of2"
                        | _ -> failwith "expected a Teq"
                }
                |> c.Apply
            | _ -> failwith "expected a SumOfProducts"

        result |> shouldEqual true

    type private TestPrivateRecord =
        {
            PrivateFoo : string
            PrivateBar : int
            PrivateBaz : string
        }

    [<Test>]
    let ``Record active pattern recognises a private record`` () =

        let r =
            {
                PrivateFoo = "hello"
                PrivateBar = 1234
                PrivateBaz = "world"
            }

        let pairs =
            tryGetStringKeyValues r |> Option.map (Map.toSeq >> Seq.sort >> List.ofSeq)

        let expected = Some [ "PrivateBaz", "world" ; "PrivateFoo", "hello" ]

        pairs |> shouldEqual expected

        let result =
            match tType<TestPrivateRecord> with
            | Record c ->
                { new RecordConvEvaluator<_, _> with
                    member _.Eval<'a> names ts (conv : Conv<TestPrivateRecord, 'a HList>) =

                        let expectedNames = [ "PrivateFoo" ; "PrivateBar" ; "PrivateBaz" ]

                        let actualNames = names |> List.map TypeField.name

                        actualNames |> shouldEqual expectedNames

                        TypeList.toTypes ts = [ typeof<string> ; typeof<int> ; typeof<string> ]
                }
                |> c.Apply
            | _ -> failwith "expected a record"

        result |> shouldEqual true

    type TestInternallyPrivateRecord =
        private
            {
                InternallyPrivateFoo : string
                InternallyPrivateBar : int
                InternallyPrivateBaz : string
            }

    [<Test>]
    let ``Record active pattern recognises a public record whose fields are private`` () =
        let r =
            {
                InternallyPrivateFoo = "hello"
                InternallyPrivateBar = 1234
                InternallyPrivateBaz = "world"
            }

        let pairs =
            tryGetStringKeyValues r |> Option.map (Map.toSeq >> Seq.sort >> List.ofSeq)

        let expected =
            Some [ "InternallyPrivateBaz", "world" ; "InternallyPrivateFoo", "hello" ]

        pairs |> shouldEqual expected

        let result =
            match tType<TestInternallyPrivateRecord> with
            | Record c ->
                { new RecordConvEvaluator<_, _> with
                    member _.Eval<'a> names ts (conv : Conv<TestInternallyPrivateRecord, 'a HList>) =

                        let expectedNames =
                            [ "InternallyPrivateFoo" ; "InternallyPrivateBar" ; "InternallyPrivateBaz" ]

                        let actualNames = names |> List.map TypeField.name

                        actualNames |> shouldEqual expectedNames

                        TypeList.toTypes ts = [ typeof<string> ; typeof<int> ; typeof<string> ]
                }
                |> c.Apply
            | _ -> failwith "expected a record"

        result |> shouldEqual true
