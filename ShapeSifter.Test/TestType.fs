namespace ShapeSifter.Test

open System
open NUnit.Framework
open FsUnitTyped
open ShapeSifter

[<TestFixture>]
module TestType =

    let testCases =
        [
            typeof<Type>, "Type"
            typeof<int>, "int"
            typeof<int64>, "int64"
            typeof<float>, "float"
            typeof<char>, "char"
            typeof<string>, "string"
            typeof<int list>, "int list"
            typeof<bool array>, "bool array"
            typeof<string seq>, "string seq"
            typeof<Map<int, string>>, "Map<int, string>"
            typeof<int -> string>, "(int -> string)"
            typeof<int * bool * string>, "(int * bool * string)"
        ]
        |> List.map (fun (t, expected) -> [| box t ; box expected |])

    [<Test>]
    [<TestCaseSource (nameof testCases)>]
    let ``Test print`` (t : Type, expected : string) : unit = Type.print t |> shouldEqual expected
