namespace ShapeSifter.Test

open ApiSurface
open ShapeSifter
open Xunit

module TestSurface =
    let assembly = typeof<TypeListCrate>.Assembly

    [<Fact>]
    let ``Ensure API surface has not been modified`` () = ApiSurface.assertIdentical assembly

    [<Fact(Skip = "Run this explicitly to update the surface baseline")>]
    let ``Update API surface`` () =
        ApiSurface.writeAssemblyBaseline assembly

    [<Fact>]
    let ``Ensure public API is fully documented`` () =
        DocCoverage.assertFullyDocumented assembly

    [<Fact>]
    let ``Ensure version is monotonic`` () =
        MonotonicVersion.validate assembly "ShapeSifter"
