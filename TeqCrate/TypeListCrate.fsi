namespace TeqCrate

open HCollections
open System

/// A universally quantified function that takes a TypeList and returns a value of type 'ret
type TypeListEvaluator<'ret> =
    /// This is the function that you wish to evaluate when you visit a `TypeListCrate` using this evaluator.
    abstract Eval : 'ts TypeList -> 'ret

/// An encoding of an existentially quantified TypeList.
/// Given a TypeListEvaluator, it will invoke it with the TypeList
/// that it holds and will return the result.
type TypeListCrate =
    /// Visit this crate with the given evaluator to reveal the type parameters within the crate.
    abstract Apply : TypeListEvaluator<'ret> -> 'ret

/// An encoding of an existentially quantified TypeList.
/// Given a TypeListEvaluator, it will invoke it with the TypeList
/// that it holds and will return the result.
module TypeListCrate =

    /// Given a TypeList, creates a TypeListCrate that holds the TypeList
    val make : 'ts TypeList -> TypeListCrate

    /// Given a list of runtime Types, creates the corresponding
    /// TypeList and then wraps it in a TypeListCrate to hide the
    /// generic type parameter.
    val makeUntyped : Type list -> TypeListCrate
