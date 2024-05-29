namespace ShapeSifter

open System.Collections.Generic
open ShapeSifter.TypePatterns
open TypeEquality


type ArrayTeqEvaluator<'a, 'ret> =
    abstract member Eval : Teq<'a, 'b array> -> 'ret

type 'a ArrayTeqCrate =
    abstract member Apply : ArrayTeqEvaluator<'a, 'ret> -> 'ret

[<RequireQualifiedAccess>]
module ArrayTeqCrate =

    let make () =
        { new ArrayTeqCrate<_> with
            member __.Apply e = e.Eval Teq.refl
        }

    let private make' = lazy (Reflection.invokeStaticMethod <@ make @>)

    let tryMake () =
        match typeof<'a> with
        | Array e -> make'.Force () [| e |] [||] |> unbox<'a ArrayTeqCrate> |> Some
        | _ -> None


type ListTeqEvaluator<'a, 'ret> =
    abstract member Eval : Teq<'a, 'b list> -> 'ret

type 'a ListTeqCrate =
    abstract member Apply : ListTeqEvaluator<'a, 'ret> -> 'ret

[<RequireQualifiedAccess>]
module ListTeqCrate =

    let make () =
        { new ListTeqCrate<_> with
            member __.Apply e = e.Eval Teq.refl
        }

    let private make' = lazy (Reflection.invokeStaticMethod <@ make @>)

    let tryMake () =
        match typeof<'a> with
        | Generic (t, ts) when t = typedefof<_ list> -> make'.Force () ts [||] |> unbox<'a ListTeqCrate> |> Some
        | _ -> None


type SeqTeqEvaluator<'a, 'ret> =
    abstract member Eval : Teq<'a, 'b seq> -> 'ret

type 'a SeqTeqCrate =
    abstract member Apply : SeqTeqEvaluator<'a, 'ret> -> 'ret

[<RequireQualifiedAccess>]
module SeqTeqCrate =

    let make () =
        { new SeqTeqCrate<_> with
            member __.Apply e = e.Eval Teq.refl
        }

    let private make' = lazy (Reflection.invokeStaticMethod <@ make @>)

    let tryMake () =
        match typeof<'a> with
        | Generic (t, ts) when t = typedefof<_ seq> -> make'.Force () ts [||] |> unbox<'a SeqTeqCrate> |> Some
        | _ -> None


type OptionTeqEvaluator<'a, 'ret> =
    abstract member Eval : Teq<'a, 'b option> -> 'ret

type 'a OptionTeqCrate =
    abstract member Apply : OptionTeqEvaluator<'a, 'ret> -> 'ret

[<RequireQualifiedAccess>]
module OptionTeqCrate =

    let make () =
        { new OptionTeqCrate<_> with
            member __.Apply e = e.Eval Teq.refl
        }

    let private make' = lazy (Reflection.invokeStaticMethod <@ make @>)

    let tryMake () =
        match typeof<'a> with
        | Generic (t, ts) when t = typedefof<_ option> -> make'.Force () ts [||] |> unbox<'a OptionTeqCrate> |> Some
        | _ -> None

type Choice2TeqEvaluator<'a, 'ret> =
    abstract Eval<'b1, 'b2> : Teq<'a, Choice<'b1, 'b2>> -> 'ret

type 'a Choice2TeqCrate =
    abstract Apply : Choice2TeqEvaluator<'a, 'ret> -> 'ret

[<RequireQualifiedAccess>]
module Choice2TeqCrate =

    let make () =
        { new Choice2TeqCrate<_> with
            member _.Apply e = e.Eval Teq.refl
        }

    let private make' = lazy (Reflection.invokeStaticMethod <@ make @>)

    let tryMake () =
        match typeof<'a> with
        | Generic (t, ts) when t = typedefof<Choice<_, _>> ->
            make'.Force () ts [||] |> unbox<'a Choice2TeqCrate> |> Some
        | _ -> None

type SetTeqEvaluator<'a, 'ret> =
    abstract member Eval : Teq<'a, 'b Set> -> 'ret

type 'a SetTeqCrate =
    abstract member Apply : SetTeqEvaluator<'a, 'ret> -> 'ret

[<RequireQualifiedAccess>]
module SetTeqCrate =

    let make<'a when 'a : comparison> () =
        { new SetTeqCrate<'a Set> with
            member __.Apply e = e.Eval Teq.refl
        }

    let private make' = lazy (Reflection.invokeStaticMethod <@ make @>)

    let tryMake () =
        match typeof<'a> with
        | Generic (t, ts) when t = typedefof<_ Set> -> make'.Force () ts [||] |> unbox<'a SetTeqCrate> |> Some
        | _ -> None


type MapTeqEvaluator<'a, 'ret> =
    abstract member Eval : Teq<'a, Map<'k, 'v>> -> 'ret

type 'a MapTeqCrate =
    abstract member Apply : MapTeqEvaluator<'a, 'ret> -> 'ret

[<RequireQualifiedAccess>]
module MapTeqCrate =

    let make<'k, 'v when 'k : comparison> () =
        { new MapTeqCrate<Map<'k, 'v>> with
            member __.Apply e = e.Eval Teq.refl
        }

    let private make' = lazy (Reflection.invokeStaticMethod <@ make @>)

    let tryMake () =
        match typeof<'a> with
        | Generic (t, ts) when t = typedefof<Map<_, _>> -> make'.Force () ts [||] |> unbox<'a MapTeqCrate> |> Some
        | _ -> None


type DictionaryTeqEvaluator<'a, 'ret> =
    abstract member Eval : Teq<'a, Dictionary<'k, 'v>> -> 'ret

type 'a DictionaryTeqCrate =
    abstract member Apply : DictionaryTeqEvaluator<'a, 'ret> -> 'ret

[<RequireQualifiedAccess>]
module DictionaryTeqCrate =

    let make () =
        { new DictionaryTeqCrate<_> with
            member __.Apply e = e.Eval Teq.refl
        }

    let private make' = lazy (Reflection.invokeStaticMethod <@ make @>)

    let tryMake () =
        match typeof<'a> with
        | Generic (t, ts) when t = typedefof<Dictionary<_, _>> ->
            make'.Force () ts [||] |> unbox<'a DictionaryTeqCrate> |> Some
        | _ -> None


type ResizeArrayTeqEvaluator<'a, 'ret> =
    abstract member Eval : Teq<'a, 'b ResizeArray> -> 'ret

type 'a ResizeArrayTeqCrate =
    abstract member Apply : ResizeArrayTeqEvaluator<'a, 'ret> -> 'ret

[<RequireQualifiedAccess>]
module ResizeArrayTeqCrate =

    let make () =
        { new ResizeArrayTeqCrate<_> with
            member __.Apply e = e.Eval Teq.refl
        }

    let private make' = lazy (Reflection.invokeStaticMethod <@ make @>)

    let tryMake () =
        match typeof<'a> with
        | Generic (t, ts) when t = typedefof<_ ResizeArray> ->
            make'.Force () ts [||] |> unbox<'a ResizeArrayTeqCrate> |> Some
        | _ -> None


type FunTeqEvaluator<'a, 'ret> =
    abstract member Eval : Teq<'a, 'b -> 'c> -> 'ret

type 'a FunTeqCrate =
    abstract member Apply : FunTeqEvaluator<'a, 'ret> -> 'ret

[<RequireQualifiedAccess>]
module FunTeqCrate =

    let make () =
        { new FunTeqCrate<_> with
            member __.Apply e = e.Eval Teq.refl
        }

    let private make' = lazy (Reflection.invokeStaticMethod <@ make @>)

    let tryMake () =
        match typeof<'a> with
        | Fun (dom, ran) -> make'.Force () [| dom ; ran |] [||] |> unbox<'a FunTeqCrate> |> Some
        | _ -> None


type PairTeqEvaluator<'a, 'ret> =
    abstract member Eval : Teq<'a, 'b * 'c> -> 'ret

type 'a PairTeqCrate =
    abstract member Apply : PairTeqEvaluator<'a, 'ret> -> 'ret

[<RequireQualifiedAccess>]
module PairTeqCrate =

    let make () =
        { new PairTeqCrate<_> with
            member __.Apply e = e.Eval Teq.refl
        }

    let private make' = lazy (Reflection.invokeStaticMethod <@ make @>)

    let tryMake () =
        match typeof<'a> with
        | Tuple ts when ts.Length = 2 -> make'.Force () ts [||] |> unbox<'a PairTeqCrate> |> Some
        | _ -> None


type TripleTeqEvaluator<'a, 'ret> =
    abstract member Eval : Teq<'a, 'b * 'c * 'd> -> 'ret

type 'a TripleTeqCrate =
    abstract member Apply : TripleTeqEvaluator<'a, 'ret> -> 'ret

[<RequireQualifiedAccess>]
module TripleTeqCrate =

    let make () =
        { new TripleTeqCrate<_> with
            member __.Apply e = e.Eval Teq.refl
        }

    let private make' = lazy (Reflection.invokeStaticMethod <@ make @>)

    let tryMake () =
        match typeof<'a> with
        | Tuple ts when ts.Length = 3 -> make'.Force () ts [||] |> unbox<'a TripleTeqCrate> |> Some
        | _ -> None
