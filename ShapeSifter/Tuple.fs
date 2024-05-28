namespace ShapeSifter

open HCollections
open ShapeSifter.Patterns

[<RequireQualifiedAccess>]
module Tuple =

    let tryFoldTuple (folder : 'state HListFolder) (seed : 'state) (tuple : 'tuple) : 'state option =
        match tType<'tuple> with
        | Tuple c ->
            c.Apply
                { new TupleConvEvaluator<_, _> with
                    member __.Eval _ conv =
                        tuple |> conv.To |> HList.fold folder seed
                }
            |> Some
        | _ -> None
