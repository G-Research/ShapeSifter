namespace ShapeSifter

open System

type TypeParameterEvaluator<'ret> =
    abstract member Eval<'a> : unit -> 'ret

type TypeParameterCrate =
    abstract member Apply : TypeParameterEvaluator<'ret> -> 'ret

[<RequireQualifiedAccess>]
module TypeParameterCrate =

    let make<'a> =
        { new TypeParameterCrate with
            member __.Apply e = e.Eval<'a> ()
        }

    let makeUntyped (t : Type) =
        Reflection.invokeStaticMethod <@ make @> [| t |] [||]
        |> unbox<TypeParameterCrate>

    let toType (crate : TypeParameterCrate) =
        crate.Apply
            { new TypeParameterEvaluator<_> with
                member __.Eval<'a> () = typeof<'a>
            }
