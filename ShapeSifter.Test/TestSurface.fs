namespace ShapeSifter.Test

open ApiSurface
open NUnit.Framework
open ShapeSifter

[<TestFixture>]
module TestSurface =
    let assembly = typeof<TypeListCrate>.Assembly

    [<Test>]
    let ``Ensure API surface has not been modified`` () = ApiSurface.assertIdentical assembly

    [<Test>]
    [<Explicit "Run this explicitly to update the surface baseline">]
    let ``Update API surface`` () =
        ApiSurface.writeAssemblyBaseline assembly

    [<Test>]
    let ``Ensure public API is fully documented`` () =
        DocCoverage.assertFullyDocumented assembly

(*
    [<Test>]
    let ``Ensure version is monotonic`` () =
        MonotonicVersion.validate assembly "ShapeSifter"
        *)
