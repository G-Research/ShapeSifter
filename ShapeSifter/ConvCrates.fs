namespace TeqCrate

open System.Reflection
open HCollections
open Microsoft.FSharp.Reflection
open System
open TypePatterns

type TupleConvEvaluator<'tuple, 'ret> =
    abstract member Eval : 'ts TypeList -> Conv<'tuple, 'ts HList> -> 'ret

type 'tuple TupleConvCrate =
    abstract member Apply : TupleConvEvaluator<'tuple, 'ret> -> 'ret

module TupleConvCrate =

    let make ts conv =
        { new TupleConvCrate<_> with
            member __.Apply e = e.Eval ts conv
        }

    let rec makeUntyped (ts : Type list) : obj list TupleConvCrate =
        match ts with
        | [] ->
            let toF _ = HList.empty
            let fromF _ = []
            Conv.make toF fromF |> make TypeList.empty
        | t :: ts ->
            let crate = TypeParameterCrate.makeUntyped t

            crate.Apply
                { new TypeParameterEvaluator<_> with
                    member __.Eval<'t> () = makeUntypedInner<'t> ts
                }

    and makeUntypedInner<'t> (ts : Type list) : obj list TupleConvCrate =
        let crate = makeUntyped ts

        crate.Apply
            { new TupleConvEvaluator<_, _> with
                member __.Eval ts conv =
                    let toF os =
                        let t = os |> List.head |> unbox<'t>
                        os |> List.tail |> conv.To |> HList.cons t

                    let fromF xs =
                        let o = xs |> HList.head |> box
                        let os = xs |> HList.tail |> conv.From
                        o :: os

                    Conv.make toF fromF |> make (ts |> TypeList.cons)
            }

    let tryMake () : 'tuple TupleConvCrate option =

        let t = typeof<'tuple>

        match t with
        | Tuple ts ->
            let tupleConv =
                let make = FSharpValue.PreComputeTupleConstructor t
                let reader = FSharpValue.PreComputeTupleReader t
                Conv.make (reader >> Array.toList) (List.toArray >> make >> unbox)

            let crate = makeUntyped ts

            crate.Apply
                { new TupleConvEvaluator<_, _> with
                    member __.Eval ts conv = Conv.compose tupleConv conv |> make ts
                }
            |> Some

        | _ -> None

type RecordConvEvaluator<'record, 'ret> =
    abstract member Eval : RecordTypeField list -> 'ts TypeList -> Conv<'record, 'ts HList> -> 'ret

type 'record RecordConvCrate =
    abstract member Apply : RecordConvEvaluator<'record, 'ret> -> 'ret

module RecordConvCrate =

    let make names tl conv =
        { new RecordConvCrate<_> with
            member __.Apply e = e.Eval names tl conv
        }

    let rec makeUntyped<'ts>
        (fields : RecordTypeField list)
        (tl : 'ts TypeList)
        (ts : Type list)
        : obj list RecordConvCrate
        =
        match ts with
        | [] ->
            let toF _ = HList.empty
            let fromF _ = []
            Conv.make toF fromF |> make fields TypeList.empty
        | t :: ts ->
            let crate = TypeParameterCrate.makeUntyped t

            crate.Apply
                { new TypeParameterEvaluator<_> with
                    member __.Eval<'t> () = makeUntypedInner<'t, 'ts> fields tl ts
                }

    and makeUntypedInner<'t, 'ts>
        (fields : RecordTypeField list)
        (tl : 'ts TypeList)
        (ts : Type list)
        : obj list RecordConvCrate
        =
        let crate = makeUntyped fields tl ts

        crate.Apply
            { new RecordConvEvaluator<_, _> with
                member __.Eval names tl conv =
                    let toF os =
                        let t = os |> List.head |> unbox<'t>
                        os |> List.tail |> conv.To |> HList.cons t

                    let fromF xs =
                        let o = xs |> HList.head |> box
                        let os = xs |> HList.tail |> conv.From
                        o :: os

                    Conv.make toF fromF |> make names (tl |> TypeList.cons)
            }

    let tryMake () : 'record RecordConvCrate option =

        let t = typeof<'record>

        match t with
        | Record ts ->
            let recordConv =
                let flags = BindingFlags.Public ||| BindingFlags.NonPublic
                let make = FSharpValue.PreComputeRecordConstructor (t, flags)
                let reader = FSharpValue.PreComputeRecordReader (t, flags)
                Conv.make (reader >> Array.toList) (List.toArray >> make >> unbox)

            let names, ts = ts |> List.unzip
            let crate = makeUntyped names TypeList.empty ts

            crate.Apply
                { new RecordConvEvaluator<_, _> with
                    member __.Eval names tl conv =
                        Conv.compose recordConv conv |> make names tl
                }
            |> Some

        | _ -> None


type UnionConvEvaluator<'union, 'ret> =
    abstract member Eval : UnionTypeField list -> 'ts TypeList -> Conv<'union, 'ts HUnion> -> 'ret

type 'union UnionConvCrate =
    abstract member Apply : UnionConvEvaluator<'union, 'ret> -> 'ret

module UnionConvCrate =

    let private flags = BindingFlags.Public ||| BindingFlags.NonPublic

    let make names ts conv =
        { new UnionConvCrate<_> with
            member __.Apply e = e.Eval names ts conv
        }

    let rec makeUntyped (names : UnionTypeField list) (ts : Type list) : (int * obj) UnionConvCrate =
        match ts with
        | [] -> failwith "Cannot create UnionConvCrate - the list of union cases must not be empty"
        | t :: ts ->

        let crate = TypeParameterCrate.makeUntyped t

        crate.Apply
            { new TypeParameterEvaluator<_> with
                member __.Eval<'t> () = makeUntypedInner<'t> names ts
            }

    and makeUntypedInner<'t> (names : UnionTypeField list) (ts : Type list) : (int * obj) UnionConvCrate =
        match ts with
        | [] ->
            let toF (_, o) =
                o |> unbox<'t> |> HUnion.make TypeList.empty

            let fromF xs = 0, HUnion.getSingleton xs |> box
            let ts = TypeList.empty |> TypeList.cons
            Conv.make toF fromF |> make names ts
        | _ ->
            let crate = makeUntyped names ts

            crate.Apply
                { new UnionConvEvaluator<_, _> with
                    member __.Eval names ts conv =
                        let toF (i, o) =
                            if i = 0 then
                                o |> unbox<'t> |> HUnion.make ts
                            else
                                (i - 1, o) |> conv.To |> HUnion.extend

                        let fromF (xs : ('t -> 'a) HUnion) : int * obj =
                            match xs |> HUnion.split with
                            | Choice1Of2 x -> 0, x |> box
                            | Choice2Of2 xs ->

                            let (i, x) = conv.From xs
                            i + 1, x

                        let ts = ts |> TypeList.cons
                        Conv.make toF fromF |> make names ts
                }

    let tryMake () : 'union UnionConvCrate option =

        let t = typeof<'union>

        match t with
        | Union cases ->

            let makeConverter =
                function
                | [] ->
                    let conv = Conv.make (fun _ -> () |> box) (fun _ -> [||])
                    typeof<unit>, conv
                | [ t ] ->
                    let conv = Conv.make Array.head Array.singleton
                    t, conv
                | ts ->
                    let t = ts |> Array.ofList |> FSharpType.MakeTupleType
                    let maker = FSharpValue.PreComputeTupleConstructor t
                    let reader = FSharpValue.PreComputeTupleReader t
                    let conv = Conv.make maker reader
                    t, conv

            let makeCaseConv case : Conv<obj, obj array> =
                let reader = FSharpValue.PreComputeUnionReader (case, flags)
                let maker = FSharpValue.PreComputeUnionConstructor (case, flags)
                Conv.make reader maker

            let bitsForCase (case : UnionCaseInfo) =
                let ts = case.GetFields () |> Array.map (fun pi -> pi.PropertyType) |> List.ofArray

                let t, conv = makeConverter ts
                t, Conv.compose (makeCaseConv case) conv

            let ts, convs = cases |> List.map bitsForCase |> List.unzip
            let convs = convs |> Array.ofList

            let unionConv : Conv<'union, int * obj> =
                let getTag = FSharpValue.PreComputeUnionTagReader (t, flags)
                let toF u = let i = getTag u in i, convs.[i].To u
                let fromF (i, o) = o |> convs.[i].From |> unbox
                Conv.make toF fromF

            let fields =
                cases
                |> List.map (fun case ->
                    let name = case.Name
                    let attributes = case.GetCustomAttributesData () |> List.ofSeq

                    {
                        Name = name
                        Attributes = attributes
                        RawCase = case
                    }
                )

            let crate = makeUntyped fields ts

            crate.Apply
                { new UnionConvEvaluator<_, _> with
                    member __.Eval fields ts conv =
                        Conv.compose unionConv conv |> make fields ts
                }
            |> Some

        | _ -> None


type SumOfProductsConvEvaluator<'union, 'ret> =
    abstract member Eval : string list -> 'tss TypeListList -> Conv<'union, 'tss SumOfProducts> -> 'ret

type 'union SumOfProductsConvCrate =
    abstract member Apply : SumOfProductsConvEvaluator<'union, 'ret> -> 'ret

module SumOfProductsConvCrate =

    let private flags = BindingFlags.Public ||| BindingFlags.NonPublic

    let make names tss conv =
        { new SumOfProductsConvCrate<_> with
            member __.Apply e = e.Eval names tss conv
        }

    let rec makeUntyped (names : string list) (tss : TypeListCrate list) : (int * obj) SumOfProductsConvCrate =
        match tss with
        | [] -> failwith "Cannot create SumOfProductsConvCrate - the list of union cases must not be empty"
        | ts :: tss ->

        ts.Apply
            { new TypeListEvaluator<_> with
                member __.Eval ts = makeUntypedInner<'ts> ts names tss
            }

    and makeUntypedInner<'ts>
        (tl : 'ts TypeList)
        (names : string list)
        (tss : TypeListCrate list)
        : (int * obj) SumOfProductsConvCrate
        =
        match tss with
        | [] ->
            let toF (_, o) =
                o |> unbox<'ts HList> |> SumOfProducts.make TypeListList.empty

            let fromF xs = 0, SumOfProducts.getSingleton xs |> box
            let tl = TypeListList.empty |> TypeListList.cons tl
            Conv.make toF fromF |> make names tl
        | _ ->
            let crate = makeUntyped names tss

            crate.Apply
                { new SumOfProductsConvEvaluator<_, _> with
                    member __.Eval names tss conv =
                        let toF (i, o) =
                            if i = 0 then
                                o |> unbox<'ts HList> |> SumOfProducts.make tss
                            else
                                (i - 1, o) |> conv.To |> SumOfProducts.extend tl

                        let fromF (xs : ('ts -> 'a) SumOfProducts) : int * obj =
                            match xs |> SumOfProducts.split with
                            | Choice1Of2 x -> 0, x |> box
                            | Choice2Of2 xs ->

                            let (i, x) = conv.From xs
                            i + 1, x

                        let tss = tss |> TypeListList.cons tl
                        Conv.make toF fromF |> make names tss
                }

    let tryMake () : 'union SumOfProductsConvCrate option =

        let t = typeof<'union>

        match t with
        | Union cases ->

            let rec makeConverter (ts : Type list) : TypeListCrate * Conv<obj list, obj> =
                match ts with
                | [] ->
                    let crate = TypeListCrate.make TypeList.empty
                    let toF _ = HList.empty |> box
                    let fromF _ = []
                    crate, Conv.make toF fromF
                | t :: ts ->
                    let crate = TypeParameterCrate.makeUntyped t

                    crate.Apply
                        { new TypeParameterEvaluator<_> with
                            member __.Eval<'t> () =
                                let crate, conv = makeConverter ts

                                crate.Apply
                                    { new TypeListEvaluator<_> with
                                        member __.Eval (ts : 'ts TypeList) =
                                            let crate = ts |> TypeList.cons<'t, 'ts> |> TypeListCrate.make

                                            let toF os =
                                                let head = os |> List.head |> unbox<'t>

                                                os |> List.tail |> conv.To |> unbox<'ts HList> |> HList.cons head |> box

                                            let fromF o =
                                                let xs = o |> unbox<('t -> 'ts) HList>

                                                (xs |> HList.head |> box) :: (xs |> HList.tail |> box |> conv.From)

                                            crate, Conv.make toF fromF
                                    }
                        }

            let makeCaseConv case : Conv<obj, obj array> =
                let reader = FSharpValue.PreComputeUnionReader (case, flags)
                let maker = FSharpValue.PreComputeUnionConstructor (case, flags)
                Conv.make reader maker

            let bitsForCase (case : UnionCaseInfo) =
                let ts = case.GetFields () |> Array.map (fun pi -> pi.PropertyType) |> List.ofArray

                let crate, conv = makeConverter ts
                let conv2 = Conv.make Array.toList List.toArray
                crate, Conv.compose (makeCaseConv case) (Conv.compose conv2 conv)

            let ts, convs = cases |> List.map bitsForCase |> List.unzip
            let convs = convs |> Array.ofList

            let unionConv : Conv<'union, int * obj> =
                let getTag = FSharpValue.PreComputeUnionTagReader (t, flags)
                let toF u = let i = getTag u in i, convs.[i].To u
                let fromF (i, o) = o |> convs.[i].From |> unbox
                Conv.make toF fromF

            let names = cases |> List.map (fun case -> case.Name)
            let crate = makeUntyped names ts

            crate.Apply
                { new SumOfProductsConvEvaluator<_, _> with
                    member __.Eval names ts conv =
                        Conv.compose unionConv conv |> make names ts
                }
            |> Some

        | _ -> None
