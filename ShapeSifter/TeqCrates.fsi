namespace ShapeSifter

open System.Collections.Generic
open TypeEquality

/// The type of values that act on an ArrayTeqCrate.
/// An encoding of a universally quantified function that takes a type equality between its
/// first type parameter and a 'b array for any 'b and returns a value of type 'ret
type ArrayTeqEvaluator<'a, 'ret> =
    /// This is the function that you wish to evaluate when you visit an `ArrayTeqCrate` using this evaluator.
    abstract Eval : Teq<'a, 'b array> -> 'ret

/// An encoding of an existentially quantified type equality between 'a and a 'b Array for some 'b.
/// Given an ArrayTeqEvaluator, it will invoke it with the type equality that it holds and will return the result.
type 'a ArrayTeqCrate =
    /// Visit this crate with the given evaluator to reveal the type parameters within the crate.
    abstract Apply : ArrayTeqEvaluator<'a, 'ret> -> 'ret

/// An encoding of an existentially quantified type equality between 'a and a 'b Array for some 'b.
/// Given an ArrayTeqEvaluator, it will invoke it with the type equality that it holds and will return the result.
[<RequireQualifiedAccess>]
module ArrayTeqCrate =

    /// For any type 'a, we can create a type equality between 'a array and 'a array, by reflexivity.
    /// make creates this type equality and then wraps it in an ArrayTeqCrate.
    val make : unit -> 'a array ArrayTeqCrate

    /// For any type 'a, checks to see if 'a is actually a 'b array for some 'b.
    /// If it is, creates the type equality Teq<'a, 'b array> and then wraps it in a crate.
    /// Otherwise, returns None.
    val tryMake : unit -> 'a ArrayTeqCrate option


/// The type of values that act on a ListTeqCrate.
/// An encoding of a universally quantified function that takes a type equality between its
/// first type parameter and a 'b list for any 'b and returns a value of type 'ret
type ListTeqEvaluator<'a, 'ret> =
    /// This is the function that you wish to evaluate when you visit a `ListTeqCrate` using this evaluator.
    abstract Eval : Teq<'a, 'b list> -> 'ret

/// An encoding of an existentially quantified type equality between 'a and a 'b list for some 'b.
/// Given a ListTeqEvaluator, it will invoke it with the type equality that it holds and will return the result.
type 'a ListTeqCrate =
    /// Visit this crate with the given evaluator to reveal the type parameters within the crate.
    abstract Apply : ListTeqEvaluator<'a, 'ret> -> 'ret

/// An encoding of an existentially quantified type equality between 'a and a 'b list for some 'b.
/// Given a ListTeqEvaluator, it will invoke it with the type equality that it holds and will return the result.
[<RequireQualifiedAccess>]
module ListTeqCrate =

    /// For any type 'a, we can create a type equality between 'a list and 'a list, by reflexivity.
    /// make creates this type equality and then wraps it in a ListTeqCrate.
    val make : unit -> 'a list ListTeqCrate

    /// For any type 'a, checks to see if 'a is actually a 'b list for some 'b.
    /// If it is, creates the type equality Teq<'a, 'b list> and then wraps it in a crate.
    /// Otherwise, returns None.
    val tryMake : unit -> 'a ListTeqCrate option


/// The type of values that act on a SeqTeqCrate.
/// An encoding of a universally quantified function that takes a type equality between its
/// first type parameter and a 'b seq for any 'b and returns a value of type 'ret
type SeqTeqEvaluator<'a, 'ret> =
    /// This is the function that you wish to evaluate when you visit a `SeqTeqCrate` using this evaluator.
    abstract Eval : Teq<'a, 'b seq> -> 'ret

/// An encoding of an existentially quantified type equality between 'a and a 'b seq for some 'b.
/// Given a SeqTeqEvaluator, it will invoke it with the type equality that it holds and will return the result.
type 'a SeqTeqCrate =
    /// Visit this crate with the given evaluator to reveal the type parameters within the crate.
    abstract Apply : SeqTeqEvaluator<'a, 'ret> -> 'ret

/// An encoding of an existentially quantified type equality between 'a and a 'b seq for some 'b.
/// Given a SeqTeqEvaluator, it will invoke it with the type equality that it holds and will return the result.
[<RequireQualifiedAccess>]
module SeqTeqCrate =

    /// For any type 'a, we can create a type equality between 'a seq and 'a seq, by reflexivity.
    /// make creates this type equality and then wraps it in a SeqTeqCrate.
    val make : unit -> 'a seq SeqTeqCrate

    /// For any type 'a, checks to see if 'a is actually a 'b seq for some 'b.
    /// If it is, creates the type equality Teq<'a, 'b seq> and then wraps it in a crate.
    /// Otherwise, returns None.
    val tryMake : unit -> 'a SeqTeqCrate option


/// The type of values that act on an OptionTeqCrate.
/// An encoding of a universally quantified function that takes a type equality between its
/// first type parameter and a 'b option for any 'b and returns a value of type 'ret
type OptionTeqEvaluator<'a, 'ret> =
    /// This is the function that you wish to evaluate when you visit an `OptionTeqCrate` using this evaluator.
    abstract Eval : Teq<'a, 'b option> -> 'ret

/// An encoding of an existentially quantified type equality between 'a and a 'b option for some 'b.
/// Given an OptionTeqEvaluator, it will invoke it with the type equality that it holds and will return the result.
type 'a OptionTeqCrate =
    /// Visit this crate with the given evaluator to reveal the type parameters within the crate.
    abstract Apply : OptionTeqEvaluator<'a, 'ret> -> 'ret

/// An encoding of an existentially quantified type equality between 'a and a 'b option for some 'b.
/// Given an OptionTeqEvaluator, it will invoke it with the type equality that it holds and will return the result.
[<RequireQualifiedAccess>]
module OptionTeqCrate =

    /// For any type 'a, we can create a type equality between 'a option and 'a option, by reflexivity.
    /// make creates this type equality and then wraps it in an OptionTeqCrate.
    val make : unit -> 'a option OptionTeqCrate

    /// For any type 'a, checks to see if 'a is actually a 'b option for some 'b.
    /// If it is, creates the type equality Teq<'a, 'b option> and then wraps it in a crate.
    /// Otherwise, returns None.
    val tryMake : unit -> 'a OptionTeqCrate option

/// The type of values that act on a Choice2TeqCrate.
/// An encoding of a universally quantified function that takes a type equality between its
/// first type parameter and a Choice<'b1, 'b2> for any 'b_i and returns a value of type 'ret.
type Choice2TeqEvaluator<'a, 'ret> =
    /// This is the function that you wish to evaluate when you visit a `Choice2TeqCrate` using this evaluator.
    abstract Eval<'b1, 'b2> : Teq<'a, Choice<'b1, 'b2>> -> 'ret

/// An encoding of an existentially quantified type equality between 'a and a Choice<'b1, 'b2> for some 'b_i.
/// Given a Choice2TeqEvaluator, it will invoke it with the type equality that it holds and will return the result.
type 'a Choice2TeqCrate =
    /// Visit this crate with the given evaluator to reveal the type parameters within the crate.
    abstract Apply : Choice2TeqEvaluator<'a, 'ret> -> 'ret

/// An encoding of an existentially quantified type equality between 'a and a Choice<'b1, 'b2> for some 'b_i.
/// Given a Choice2TeqEvaluator, it will invoke it with the type equality that it holds and will return the result.
[<RequireQualifiedAccess>]
module Choice2TeqCrate =

    /// For any type 'a, we can create a type equality between Choice<'b1, 'b2> and Choice<'b1, 'b2>, by reflexivity.
    /// make creates this type equality and then wraps it in a Choice2TeqCrate.
    val make<'b1, 'b2> : unit -> Choice<'b1, 'b2> Choice2TeqCrate

    /// For any type 'a, checks to see if 'a is actually a Choice<'b1, 'b2> for some 'b_i.
    /// If it is, creates the type equality Teq<'a, Choice<'b1, 'b2>> and then wraps it in a crate.
    /// Otherwise, returns None.
    val tryMake : unit -> 'a Choice2TeqCrate option

/// The type of values that act on a SetTeqCrate.
/// An encoding of a universally quantified function that takes a type equality between its
/// first type parameter and a 'b Set for any 'b and returns a value of type 'ret
type SetTeqEvaluator<'a, 'ret> =
    /// This is the function that you wish to evaluate when you visit a `SetTeqCrate` using this evaluator.
    abstract Eval : Teq<'a, 'b Set> -> 'ret

/// An encoding of an existentially quantified type equality between 'a and a 'b Set for some 'b.
/// Given a SetTeqEvaluator, it will invoke it with the type equality that it holds and will return the result.
type 'a SetTeqCrate =
    /// Visit this crate with the given evaluator to reveal the type parameters within the crate.
    abstract Apply : SetTeqEvaluator<'a, 'ret> -> 'ret

/// An encoding of an existentially quantified type equality between 'a and a 'b Set for some 'b.
/// Given a SetTeqEvaluator, it will invoke it with the type equality that it holds and will return the result.
[<RequireQualifiedAccess>]
module SetTeqCrate =

    /// For any type 'a, we can create a type equality between 'a Set and 'a Set, by reflexivity.
    /// make creates this type equality and then wraps it in a SeqTeqCrate.
    val make<'a when 'a : comparison> : unit -> 'a Set SetTeqCrate

    /// For any type 'a, checks to see if 'a is actually a 'b Set for some 'b.
    /// If it is, creates the type equality Teq<'a, 'b Set> and then wraps it in a crate.
    /// Otherwise, returns None.
    val tryMake : unit -> 'a SetTeqCrate option


/// The type of values that act on a MapTeqCrate.
/// An encoding of a universally quantified function that takes a type equality between its
/// first type parameter and a Map<'k, 'v> for any 'k, 'v and returns a value of type 'ret
type MapTeqEvaluator<'a, 'ret> =
    /// This is the function that you wish to evaluate when you visit a `MapTeqCrate` using this evaluator.
    abstract Eval : Teq<'a, Map<'k, 'v>> -> 'ret

/// An encoding of an existentially quantified type equality between 'a and a Map<'k, 'v> for some 'k, 'v.
/// Given a MapTeqEvaluator, it will invoke it with the type equality that it holds and will return the result.
type 'a MapTeqCrate =
    /// Visit this crate with the given evaluator to reveal the type parameters within the crate.
    abstract Apply : MapTeqEvaluator<'a, 'ret> -> 'ret

/// An encoding of an existentially quantified type equality between 'a and a Map<'k, 'v> for some 'k, 'v.
/// Given a MapTeqEvaluator, it will invoke it with the type equality that it holds and will return the result.
[<RequireQualifiedAccess>]
module MapTeqCrate =

    /// For any types 'k and 'v, we can create a type equality between Map<'k, 'v> and Map<'k, 'v>, by reflexivity.
    /// make creates this type equality and then wraps it in a MapTeqCrate.
    val make<'k, 'v when 'k : comparison> : unit -> Map<'k, 'v> MapTeqCrate

    /// For any type 'a, checks to see if 'a is actually a Map<'k, 'v> for some 'k ,'v.
    /// If it is, creates the type equality Teq<'a, Map<'k, 'v>> and then wraps it in a crate.
    /// Otherwise, returns None.
    val tryMake : unit -> 'a MapTeqCrate option


/// The type of values that act on a DictionaryTeqCrate.
/// An encoding of a universally quantified function that takes a type equality between its
/// first type parameter and a Dictionary<'k, 'v> for any 'k, 'v and returns a value of type 'ret
type DictionaryTeqEvaluator<'a, 'ret> =
    /// This is the function that you wish to evaluate when you visit a `DictionaryTeqCrate` using this evaluator.
    abstract Eval : Teq<'a, Dictionary<'k, 'v>> -> 'ret

/// An encoding of an existentially quantified type equality between 'a and a Dictionary<'k, 'v> for some 'k, 'v.
/// Given a DictionaryTeqEvaluator, it will invoke it with the type equality that it holds and will return the result.
type 'a DictionaryTeqCrate =
    /// Visit this crate with the given evaluator to reveal the type parameters within the crate.
    abstract Apply : DictionaryTeqEvaluator<'a, 'ret> -> 'ret

/// An encoding of an existentially quantified type equality between 'a and a Dictionary<'k, 'v> for some 'k, 'v.
/// Given a DictionaryTeqEvaluator, it will invoke it with the type equality that it holds and will return the result.
[<RequireQualifiedAccess>]
module DictionaryTeqCrate =

    /// For any types 'k and 'v, we can create a type equality between Dictionary<'k, 'v> and Dictionary<'k, 'v>, by reflexivity.
    /// make creates this type equality and then wraps it in a DictionaryTeqCrate.
    val make : unit -> Dictionary<'k, 'v> DictionaryTeqCrate

    /// For any type 'a, checks to see if 'a is actually a Dictionary<'k, 'v> for some 'k ,'v.
    /// If it is, creates the type equality Teq<'a, Dictionary<'k, 'v>> and then wraps it in a crate.
    /// Otherwise, returns None.
    val tryMake : unit -> 'a DictionaryTeqCrate option


/// The type of values that act on a ResizeArrayTeqCrate.
/// An encoding of a universally quantified function that takes a type equality between its
/// first type parameter and a 'b ResizeArray for any 'b and returns a value of type 'ret
type ResizeArrayTeqEvaluator<'a, 'ret> =
    /// This is the function that you wish to evaluate when you visit a `ResizeArrayTeqCrate` using this evaluator.
    abstract Eval : Teq<'a, 'b ResizeArray> -> 'ret

/// An encoding of an existentially quantified type equality between 'a and a 'b ResizeArray for some 'b.
/// Given a ResizeArrayTeqEvaluator, it will invoke it with the type equality that it holds and will return the result.
type 'a ResizeArrayTeqCrate =
    /// Visit this crate with the given evaluator to reveal the type parameters within the crate.
    abstract Apply : ResizeArrayTeqEvaluator<'a, 'ret> -> 'ret

/// An encoding of an existentially quantified type equality between 'a and a 'b ResizeArray for some 'b.
/// Given a ResizeArrayTeqEvaluator, it will invoke it with the type equality that it holds and will return the result.
[<RequireQualifiedAccess>]
module ResizeArrayTeqCrate =

    /// For any type 'a, we can create a type equality between 'a ResizeArray and 'a ResizeArray, by reflexivity.
    /// make creates this type equality and then wraps it in a ResizeArrayTeqCrate.
    val make : unit -> 'a ResizeArray ResizeArrayTeqCrate

    /// For any type 'a, checks to see if 'a is actually a 'b ResizeArray for some 'b.
    /// If it is, creates the type equality Teq<'a, 'b ResizeArray> and then wraps it in a crate.
    /// Otherwise, returns None.
    val tryMake : unit -> 'a ResizeArrayTeqCrate option


/// The type of values that act on a FunTeqCrate.
/// An encoding of a universally quantified function that takes a type equality between its
/// first type parameter and the funtion type ('b -> 'c) for any 'b, 'c and returns a value of type 'ret
type FunTeqEvaluator<'a, 'ret> =
    /// This is the function that you wish to evaluate when you visit a `FunTeqCrate` using this evaluator.
    abstract Eval : Teq<'a, 'b -> 'c> -> 'ret

/// An encoding of an existentially quantified type equality between 'a and the funtion type ('b -> 'c) for some 'b, 'c.
/// Given a FunTeqEvaluator, it will invoke it with the type equality that it holds and will return the result.
type 'a FunTeqCrate =
    /// Visit this crate with the given evaluator to reveal the type parameters within the crate.
    abstract Apply : FunTeqEvaluator<'a, 'ret> -> 'ret

/// An encoding of an existentially quantified type equality between 'a and the funtion type ('b -> 'c) for some 'b, 'c.
/// Given a FunTeqEvaluator, it will invoke it with the type equality that it holds and will return the result.
[<RequireQualifiedAccess>]
module FunTeqCrate =

    /// For any types 'a and 'b, we can create a type equality between ('b -> 'c) and ('b -> 'c), by reflexivity.
    /// make creates this type equality and then wraps it in a FunTeqCrate.
    val make : unit -> ('a -> 'b) FunTeqCrate

    /// For any type 'a, checks to see if 'a is actually a function type ('b -> 'c) for some 'b ,'c.
    /// If it is, creates the type equality Teq<'a, 'b -> 'c> and then wraps it in a crate.
    /// Otherwise, returns None.
    val tryMake : unit -> 'a FunTeqCrate option


/// The type of values that act on a PairTeqCrate.
/// An encoding of a universally quantified function that takes a type equality between its
/// first type parameter and the pair type 'b * 'c for any 'b, 'c and returns a value of type 'ret
type PairTeqEvaluator<'a, 'ret> =
    /// This is the function that you wish to evaluate when you visit a `PairTeqCrate` using this evaluator.
    abstract Eval : Teq<'a, 'b * 'c> -> 'ret

/// An encoding of an existentially quantified type equality between 'a and the pair type 'b * 'c for some 'b, 'c.
/// Given a PairTeqEvaluator, it will invoke it with the type equality that it holds and will return the result.
type 'a PairTeqCrate =
    /// Visit this crate with the given evaluator to reveal the type parameters within the crate.
    abstract Apply : PairTeqEvaluator<'a, 'ret> -> 'ret

/// An encoding of an existentially quantified type equality between 'a and the pair type 'b * 'c for some 'b, 'c.
/// Given a PairTeqEvaluator, it will invoke it with the type equality that it holds and will return the result.
[<RequireQualifiedAccess>]
module PairTeqCrate =

    /// For any types 'a and 'b, we can create a type equality between 'a * 'b and 'a * 'b, by reflexivity.
    /// make creates this type equality and then wraps it in a PairTeqCrate.
    val make : unit -> ('a * 'b) PairTeqCrate

    /// For any type 'a, checks to see if 'a is actually the pair type 'b * 'c for some 'b ,'c.
    /// If it is, creates the type equality Teq<'a, 'b * 'c> and then wraps it in a crate.
    /// Otherwise, returns None.
    val tryMake : unit -> 'a PairTeqCrate option


/// The type of values that act on a TripleTeqCrate.
/// An encoding of a universally quantified function that takes a type equality between its
/// first type parameter and the triple type 'b * 'c * 'd for any 'b, 'c, 'd and returns a value of type 'ret
type TripleTeqEvaluator<'a, 'ret> =
    /// This is the function that you wish to evaluate when you visit a `TripleTeqCrate` using this evaluator.
    abstract Eval : Teq<'a, 'b * 'c * 'd> -> 'ret

/// An encoding of an existentially quantified type equality between 'a and the triple type 'b * 'c * 'd for some 'b, 'c, 'd.
/// Given a TripleTeqEvaluator, it will invoke it with the type equality that it holds and will return the result.
type 'a TripleTeqCrate =
    /// Visit this crate with the given evaluator to reveal the type parameters within the crate.
    abstract Apply : TripleTeqEvaluator<'a, 'ret> -> 'ret

/// An encoding of an existentially quantified type equality between 'a and the triple type 'b * 'c * 'd for some 'b, 'c, 'd.
/// Given a TripleTeqEvaluator, it will invoke it with the type equality that it holds and will return the result.
[<RequireQualifiedAccess>]
module TripleTeqCrate =

    /// For any types 'a, 'b and 'c, we can create a type equality between 'a * 'b * 'c and 'a * 'b * 'c, by reflexivity.
    /// make creates this type equality and then wraps it in a TripleTeqCrate.
    val make : unit -> ('a * 'b * 'c) TripleTeqCrate

    /// For any type 'a, checks to see if 'a is actually the triple type 'b * 'c * 'd for some 'b ,'c and 'd.
    /// If it is, creates the type equality Teq<'a, 'b * 'c * 'd> and then wraps it in a crate.
    /// Otherwise, returns None.
    val tryMake : unit -> 'a TripleTeqCrate option
