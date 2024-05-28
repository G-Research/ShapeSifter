namespace TeqCrate

open Microsoft.FSharp.Reflection
open System.Reflection

/// A field in a record or discriminated union.
[<NoComparison ; NoEquality>]
type TypeField<'case> =
    {
        /// The name of the field; for example, the name of this very field is "Name".
        Name : string
        /// Any attributes that were present on the field.
        Attributes : CustomAttributeData list
        /// The raw object that represents this field, obtained by reflection.
        RawCase : 'case
    }

/// A field in a record.
type RecordTypeField = TypeField<PropertyInfo>

/// A field in a discriminated union.
type UnionTypeField = TypeField<UnionCaseInfo>

[<RequireQualifiedAccess>]
module TypeField =

    /// Get the Name contained in a TypeField.
    val name<'case> : TypeField<'case> -> string

    /// Get the Attributes contained in a TypeField.
    val attributes<'case> : TypeField<'case> -> CustomAttributeData list
