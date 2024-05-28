namespace ShapeSifter

open HCollections

/// Utility module that provides helper functions for working with tuple types.
[<RequireQualifiedAccess>]
module Tuple =

    /// Given an HListFolder, an initial state and an object, tests to see if
    /// the type of the object is a tuple type. If it is, the elements of the
    /// tuple are then folded over by the HListFolder, returning the final state.
    /// Otherwise, returns None.
    val tryFoldTuple : 'state HListFolder -> 'state -> 'tuple -> 'state option
