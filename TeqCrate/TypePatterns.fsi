namespace TeqCrate

open Microsoft.FSharp.Reflection
open System

/// Contains a set of active patterns to analyse runtime Types.
module TypePatterns =

    /// Recognises types that represent a generic type with generic type parameters.
    val (|Generic|_|) : Type -> (Type * Type list) option

    /// Recognises types that represent an Array type.
    val (|Array|_|) : Type -> Type option

    /// Recognises types that represent a Tuple type.
    val (|Tuple|_|) : Type -> Type list option

    /// Recognises types that represent a Function type.
    val (|Fun|_|) : Type -> (Type * Type) option

    /// Recognises types that represent an F# record type.
    val (|Record|_|) : Type -> (RecordTypeField * Type) list option

    /// Recognises types that represent an F# discriminated union type.
    val (|Union|_|) : Type -> UnionCaseInfo list option
