namespace ShapeSifter

open System

/// Utility module that contains convenience functions for working with runtime Types.
[<RequireQualifiedAccess>]
module Type =

    /// Pretty-printer for runtime Types.
    val print : Type -> string
