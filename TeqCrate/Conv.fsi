namespace TeqCrate

/// The Conv type (short for converter) is essentially a bi-directional map between two types.
/// For any pair of types 'a and 'b, a Conv<'a, 'b> is a pair of functions
/// - one mapping from 'a -> 'b and the other mapping from 'b -> 'a.
[<NoComparison>]
[<NoEquality>]
type Conv<'a, 'b> =
    {
        /// The Conv type witnesses that 'a can be converted freely to 'b, and vice versa.
        /// This method performs the conversion in the forward direction.
        To : 'a -> 'b
        /// The Conv type witnesses that 'a can be converted freely to 'b, and vice versa.
        /// This method performs the conversion in the backward direction.
        From : 'b -> 'a
    }

/// The Conv type (short for converter) is essentially a bi-directional map between two types.
/// For any pair of types 'a and 'b, a Conv<'a, 'b> is a pair of functions
/// - one mapping from 'a -> 'b and the other mapping from 'b -> 'a.
[<RequireQualifiedAccess>]
module Conv =

    /// Takes a pair of mapping functions and returns a converter of the corresponding type
    val make : ('a -> 'b) -> ('b -> 'a) -> Conv<'a, 'b>

    /// Given two converters, which convert between 'a <-> 'b and 'b <-> 'c respectively,
    /// creates the composite converter which converts between 'a <-> 'c
    val compose : Conv<'a, 'b> -> Conv<'b, 'c> -> Conv<'a, 'c>
