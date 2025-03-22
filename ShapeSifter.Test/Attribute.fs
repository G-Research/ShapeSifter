namespace ShapeSifter.Test

open System.Reflection
open ShapeSifter

[<RequireQualifiedAccess>]
module Attribute =

    /// The F# compiler puts in a bunch of attributes on each of these cases; we're not interested in that.
    let filterFromField<'a> (field : TypeField<'a>) : CustomAttributeData list =
        field.Attributes
        |> List.filter (fun attr ->
            [
                typeof<Microsoft.FSharp.Core.CompilationMappingAttribute>
                typeof<System.SerializableAttribute>
                typeof<System.Diagnostics.DebuggerDisplayAttribute>
            ]
            |> List.forall (fun t -> attr.AttributeType <> t)
        )

    /// Remove built-in runtime attributes like CompilerGeneratedAttribute.
    let filterRuntime (attrs : CustomAttributeData list) : System.Type list =
        attrs
        |> List.choose (fun attr ->
            if attr.AttributeType.Module.Name = "System.Private.CoreLib.dll" then
                None
            else
                Some attr.AttributeType
        )
