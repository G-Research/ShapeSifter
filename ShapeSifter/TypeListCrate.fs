namespace ShapeSifter

open HCollections
open System

type TypeListEvaluator<'ret> =
    abstract member Eval : 'ts TypeList -> 'ret

type TypeListCrate =
    abstract member Apply : TypeListEvaluator<'ret> -> 'ret

module TypeListCrate =

    let make (ts : 'ts TypeList) : TypeListCrate =
        { new TypeListCrate with
            member __.Apply e = e.Eval ts
        }

    let rec makeUntyped (ts : Type list) : TypeListCrate =
        match ts with
        | [] -> TypeList.empty |> make
        | t :: ts ->

        let crate = makeUntyped ts

        crate.Apply
            { new TypeListEvaluator<_> with
                member __.Eval ts =
                    let crate = TypeParameterCrate.makeUntyped t

                    crate.Apply
                        { new TypeParameterEvaluator<_> with
                            member __.Eval<'t> () = ts |> TypeList.cons<'t, _> |> make
                        }
            }
