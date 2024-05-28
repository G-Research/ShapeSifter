namespace ShapeSifter

open Microsoft.FSharp.Quotations
open Microsoft.FSharp.Quotations.Patterns
open System

[<RequireQualifiedAccess>]
module Reflection =

    let invokeStaticMethod (e : Expr) : Type seq -> obj seq -> obj =

        let rec getMethodInfo =
            function
            | Call (_, mi, _) -> mi
            | Lambda (_, e) -> getMethodInfo e
            | _ -> failwith "Could not get MethodInfo"

        let mi = (getMethodInfo e).GetGenericMethodDefinition ()

        fun ts vs ->
            mi.MakeGenericMethod (ts |> Array.ofSeq)
            |> fun mi -> mi.Invoke (null, vs |> Array.ofSeq)
