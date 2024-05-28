namespace TeqCrate

open System

/// An encoding of a universally quantified function that takes no parameters
/// other than the type that it is invoked with.
type TypeParameterEvaluator<'ret> =
    /// This is the function that you wish to evaluate when you visit a `TypeParameterCrate` using this evaluator.
    abstract Eval<'a> : unit -> 'ret

/// An encoding of an existentially quantified type parameter.
/// Given a TypeParameterEvaluator, it will invoke it with the particular generic type
/// that it holds and will return the result.
type TypeParameterCrate =
    /// Visit this crate with the given evaluator to reveal the type parameters within the crate.
    abstract Apply : TypeParameterEvaluator<'ret> -> 'ret

/// An encoding of an existentially quantified type parameter.
/// Given a TypeParameterEvaluator, it will invoke it with the particular generic type
/// that it holds and will return the result.
[<RequireQualifiedAccess>]
module TypeParameterCrate =

    /// Given a type, as expressed by a generic type parameter, returns the
    /// TypeParameterCrate that holds that type.
    val make<'a> : TypeParameterCrate

    /// Given a type, as expressed by a runtime Type object, returns the
    /// TypeParameterCrate that holds that type.
    val makeUntyped : Type -> TypeParameterCrate

    /// Given a TypeParameterCrate, returns a runtime Type object that
    //// corresponds to the type held in the crate.
    val toType : TypeParameterCrate -> Type
