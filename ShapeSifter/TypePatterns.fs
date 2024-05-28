namespace ShapeSifter

open System.Reflection
open Microsoft.FSharp.Reflection
open System

module TypePatterns =

    let (|Generic|_|) (t : Type) : (Type * Type list) option =
        if t.IsGenericType then
            let g = t.GetGenericTypeDefinition ()
            let args = t.GetGenericArguments ()
            (g, args |> List.ofArray) |> Some
        else
            None

    let (|Array|_|) (t : Type) : Type option =
        if t.IsArray then t.GetElementType () |> Some else None

    let (|Tuple|_|) (t : Type) : Type list option =
        if FSharpType.IsTuple t then
            FSharpType.GetTupleElements t |> List.ofArray |> Some
        else
            None

    let (|Fun|_|) (t : Type) : (Type * Type) option =
        if FSharpType.IsFunction t then
            FSharpType.GetFunctionElements t |> Some
        else
            None

    let (|Record|_|) (t : Type) : (TypeField<PropertyInfo> * Type) list option =
        if FSharpType.IsRecord (t, allowAccessToPrivateRepresentation = true) then
            let pis = FSharpType.GetRecordFields (t, true)

            pis
            |> Seq.map (fun pi ->
                let field =
                    {
                        Name = pi.Name
                        Attributes = pi.CustomAttributes |> Seq.toList
                        RawCase = pi
                    }

                field, pi.PropertyType
            )
            |> List.ofSeq
            |> Some
        else
            None

    let (|Union|_|) (t : Type) : UnionCaseInfo list option =
        if FSharpType.IsUnion (t, BindingFlags.Public ||| BindingFlags.NonPublic) then
            FSharpType.GetUnionCases (t, true) |> List.ofArray |> Some
        else
            None
