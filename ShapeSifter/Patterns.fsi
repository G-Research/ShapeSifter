namespace ShapeSifter

open System
open TypeEquality

/// Contains a set of active patterns to analyse typed runtime Types.
module Patterns =

    /// TType (short for 'Typed Type') is a value of a runtime type that is also generic on its value.
    /// We pattern match on typed types (rather than just runtime types) when trying to match against
    /// our TeqCrates so that the type that we're matching against corresponds to the type in the Teq.
    type 'a TType =
        /// TType (short for 'Typed Type') is a value of a runtime type that is also generic on its value.
        /// We pattern match on typed types (rather than just runtime types) when trying to match against
        /// our TeqCrates so that the type that we're matching against corresponds to the type in the Teq.
        | TType of unit

    /// Single constructor for TType - creates a TType value of 'a when invoked with any generic
    /// type parameter 'a
    [<RequiresExplicitTypeArguments>]
    val tType<'a> : 'a TType

    /// Recognises tTypes that represent the bool type.
    val (|Bool|_|) : 'a TType -> Teq<'a, bool> option

    /// Recognises tTypes that represent the int type.
    val (|Int|_|) : 'a TType -> Teq<'a, int> option

    /// Recognises tTypes that represent the int64 type.
    val (|Int64|_|) : 'a TType -> Teq<'a, int64> option

    /// Recognises tTypes that represent the float type.
    val (|Float|_|) : 'a TType -> Teq<'a, float> option

    /// Recognises tTypes that represent the string type.
    val (|String|_|) : 'a TType -> Teq<'a, string> option

    /// Recognises tTypes that represent the unit type.
    val (|Unit|_|) : 'a TType -> Teq<'a, unit> option

    /// Recognises tTypes that represent the DateTime type.
    val (|DateTime|_|) : 'a TType -> Teq<'a, DateTime> option

    /// Recognises tTypes that represent the TimeSpan type.
    val (|TimeSpan|_|) : 'a TType -> Teq<'a, TimeSpan> option

    /// Recognises tTypes that match the second given tType.
    val (|Teq|_|) : 'b TType -> 'a TType -> Teq<'a, 'b> option

    /// Recognises tTypes that represent an Array type.
    val (|Array|_|) : 'a TType -> 'a ArrayTeqCrate option

    /// Recognises tTypes that represent a list type.
    val (|List|_|) : 'a TType -> 'a ListTeqCrate option

    /// Recognises tTypes that represent a seq type.
    val (|Seq|_|) : 'a TType -> 'a SeqTeqCrate option

    /// Recognises tTypes that represent an option type.
    val (|Option|_|) : 'a TType -> 'a OptionTeqCrate option

    /// Recognises tTypes that represent a Set type.
    val (|Set|_|) : 'a TType -> 'a SetTeqCrate option

    /// Recognises tTypes that represent a Map type.
    val (|Map|_|) : 'a TType -> 'a MapTeqCrate option

    /// Recognises tTypes that represent a Dictionary type.
    val (|Dictionary|_|) : 'a TType -> 'a DictionaryTeqCrate option

    /// Recognises tTypes that represent a ResizeArray type.
    val (|ResizeArray|_|) : 'a TType -> 'a ResizeArrayTeqCrate option

    /// Recognises tTypes that represent a function type.
    val (|Fun|_|) : 'a TType -> 'a FunTeqCrate option

    /// Recognises tTypes that represent a pair type.
    val (|Pair|_|) : 'a TType -> 'a PairTeqCrate option

    /// Recognises tTypes that represent a triple type.
    val (|Triple|_|) : 'a TType -> 'a TripleTeqCrate option

    /// Recognises tTypes that represent a tuple type.
    val (|Tuple|_|) : 'a TType -> 'a TupleConvCrate option

    /// Recognises tTypes that represent an F# record type.
    val (|Record|_|) : 'a TType -> 'a RecordConvCrate option

    /// Recognises tTypes that represent an F# discriminated union type.
    val (|Union|_|) : 'a TType -> 'a UnionConvCrate option

    /// Recognises tTypes that represent an F# discriminated union type,
    /// returning a converter to decompose it into a SumOfProducts type.
    val (|SumOfProducts|_|) : 'a TType -> 'a SumOfProductsConvCrate option
