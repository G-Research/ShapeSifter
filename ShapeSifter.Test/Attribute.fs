namespace TeqCrate.Test

open TeqCrate

[<RequireQualifiedAccess>]
module Attribute =

    /// The F# compiler puts in a bunch of attributes on each of these cases; we're not interested in that.
    let filterFromField<'a> (field : TypeField<'a>) =
        field.Attributes
        |> List.filter (fun attr ->
            [
                typeof<Microsoft.FSharp.Core.CompilationMappingAttribute>
                typeof<System.SerializableAttribute>
                typeof<System.Diagnostics.DebuggerDisplayAttribute>
            ]
            |> List.forall (fun t -> attr.AttributeType <> t)
        )
