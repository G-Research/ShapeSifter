namespace ShapeSifter

open HCollections

/// The type of values that act on an TupleConvCrate.
/// An encoding of a universally quantified function that takes a TypeList and a converter between
/// first type parameter 'tuple and a 'ts HList for any 'ts and returns a value of type 'ret
type TupleConvEvaluator<'tuple, 'ret> =
    /// This is the function that you wish to evaluate when you visit a `TupleConvCrate` using this evaluator.
    abstract Eval : 'ts TypeList -> Conv<'tuple, 'ts HList> -> 'ret

/// An encoding of an existentially quantified converter between 'tuple and 'ts HList for some 'ts.
/// Given a TupleConvEvaluator, it will invoke it with the TypeList and HList that it holds and will return the result.
type 'tuple TupleConvCrate =
    /// Visit this crate with the given evaluator to reveal the type parameters within the crate.
    abstract Apply : TupleConvEvaluator<'tuple, 'ret> -> 'ret

/// An encoding of an existentially quantified converter between 'tuple and 'ts HList for some 'ts.
/// Given a TupleConvEvaluator, it will invoke it with the TypeList and HList that it holds and will return the result.
module TupleConvCrate =

    /// For any type 'tuple, checks to see if 'tuple is actually a tuple type 'a1 * 'a1 ... * 'an for some 'a1 ... 'an.
    /// If it is, creates a converter Conv<'tuple, 'a1 * 'a2 * ... 'an> and then wraps it in a crate.
    /// Otherwise, returns None.
    val tryMake : unit -> 'tuple TupleConvCrate option

/// The type of values that act on an RecordConvCrate.
/// An encoding of a universally quantified function that takes a list of record fields,
/// a TypeList, and a converter between the first type parameter 'record and a 'ts HList
/// for any 'ts; the function returns a value of type 'ret.
type RecordConvEvaluator<'record, 'ret> =
    /// This is the function that you wish to evaluate when you visit a `RecordConvCrate` using this evaluator.
    abstract Eval<'ts> : RecordTypeField list -> 'ts TypeList -> Conv<'record, 'ts HList> -> 'ret

/// An encoding of an existentially quantified converter between 'record and 'ts HList for some 'ts.
/// Given a RecordConvEvaluator, it will invoke it with the field names, TypeList and HList that it holds and will return the result.
type 'record RecordConvCrate =
    /// Visit this crate with the given evaluator to reveal the type parameters within the crate.
    abstract Apply : RecordConvEvaluator<'record, 'ret> -> 'ret

/// An encoding of an existentially quantified converter between 'record and 'ts HList for some 'ts.
/// Given a RecordConvEvaluator, it will invoke it with the field names, TypeList and HList that it holds and will return the result.
module RecordConvCrate =

    /// For any type 'record, checks to see if 'record is actually an F# record type.
    /// If it is, creates a converter Conv<'record, 'ts HList> and then wraps it in a crate.
    /// Otherwise, returns None.
    val tryMake : unit -> 'record RecordConvCrate option


/// The type of values that act on an UnionConvCrate.
/// An encoding of a universally quantified function that takes a list of fields corresponding to
/// union cases of the discriminated union, a TypeList, and a converter between
/// first type parameter 'union and a 'ts HUnion for any 'ts and returns a value of type 'ret
type UnionConvEvaluator<'union, 'ret> =
    /// This is the function that you wish to evaluate when you visit a `UnionConvCrate` using this evaluator.
    abstract Eval : UnionTypeField list -> 'ts TypeList -> Conv<'union, 'ts HUnion> -> 'ret

/// An encoding of an existentially quantified converter between 'union and 'ts HUnion for some 'ts.
/// Given a UnionConvEvaluator, it will invoke it with the case names, TypeList and HUnion that it holds and will return the result.
type 'union UnionConvCrate =
    /// Visit this crate with the given evaluator to reveal the type parameters within the crate.
    abstract Apply : UnionConvEvaluator<'union, 'ret> -> 'ret

/// An encoding of an existentially quantified converter between 'union and 'ts HUnion for some 'ts.
/// Given a UnionConvEvaluator, it will invoke it with the case names, TypeList and HUnion that it holds and will return the result.
module UnionConvCrate =

    /// For any type 'union, checks to see if 'union is actually an F# discriminated union type.
    /// If it is, creates a converter Conv<'union, 'ts HUnion> and then wraps it in a crate.
    /// Otherwise, returns None.
    val tryMake : unit -> 'union UnionConvCrate option


/// The type of values that act on an SumOfProductsConvCrate.
/// An encoding of a universally quantified function that takes a list of strings corresponding to
/// the names of the union cases of the discriminated union, a TypeListList and a converter between
/// first type parameter 'union and a 'tss SumOfProducts for any 'tss and returns a value of type 'ret
type SumOfProductsConvEvaluator<'union, 'ret> =
    /// This is the function that you wish to evaluate when you visit a `SumOfProductsConvCrate` using this evaluator.
    abstract Eval : string list -> 'tss TypeListList -> Conv<'union, 'tss SumOfProducts> -> 'ret

/// An encoding of an existentially quantified converter between 'union and 'tss SumOfProducts for some 'tss.
/// Given a SumOfProductsConvEvaluator, it will invoke it with the case names, TypeList and SumOfProducts that it holds and will return the result.
type 'union SumOfProductsConvCrate =
    /// Visit this crate with the given evaluator to reveal the type parameters within the crate.
    abstract Apply : SumOfProductsConvEvaluator<'union, 'ret> -> 'ret

/// An encoding of an existentially quantified converter between 'union and 'tss SumOfProducts for some 'tss.
/// Given a SumOfProductsConvEvaluator, it will invoke it with the case names, TypeList and SumOfProducts that it holds and will return the result.
module SumOfProductsConvCrate =

    /// For any type 'union, checks to see if 'union is actually an F# discriminated union type.
    /// If it is, creates a converter Conv<'union, 'tss SumOfProducts> and then wraps it in a crate.
    /// Otherwise, returns None.
    val tryMake : unit -> 'union SumOfProductsConvCrate option
