namespace ShapeSifter

open Microsoft.FSharp.Reflection
open System.Reflection

[<NoComparison ; NoEquality>]
type TypeField<'case> =
    {
        Name : string
        Attributes : CustomAttributeData list
        RawCase : 'case
    }

type RecordTypeField = TypeField<PropertyInfo>
type UnionTypeField = TypeField<UnionCaseInfo>

[<RequireQualifiedAccess>]
module TypeField =

    let name (field : TypeField<_>) : string = field.Name

    let attributes (field : TypeField<_>) : CustomAttributeData list = field.Attributes
