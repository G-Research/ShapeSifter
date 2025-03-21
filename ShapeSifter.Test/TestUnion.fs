namespace ShapeSifter.Test

open System

open System.Reflection
open NUnit.Framework
open FsUnitTyped

open ShapeSifter
open ShapeSifter.Patterns

[<TestFixture>]
module TestUnion =

    [<AttributeUsage(AttributeTargets.Method ||| AttributeTargets.Property, Inherited = true)>]
    type Foo () =
        inherit Attribute ()

    [<AttributeUsage(AttributeTargets.All, Inherited = false)>]
    type Bar () =
        inherit Attribute ()

    type UnionType =
        | Case1 of string
        | [<Foo>] Case2 of int
        | [<Bar>] Case3
        | [<Foo ; Bar>] Case4 of int64

    [<Test>]
    let ``Custom attributes are populated correctly for union cases`` () =
        let data =
            match tType<UnionType> with
            | Union data -> data
            | _ -> failwith "Unexpected type"

        let fields =
            { new UnionConvEvaluator<UnionType, _> with
                member _.Eval (fields : UnionTypeField list) _ _ = fields
            }
            |> data.Apply

        let attributes =
            fields
            |> List.map (fun field -> field.Name, Attribute.filterFromField field)
            |> Map.ofList

        attributes.Count |> shouldEqual 4

        attributes.["Case1"] |> Attribute.filterRuntime |> shouldBeEmpty

        attributes.["Case2"]
        |> Attribute.filterRuntime
        |> List.exactlyOne
        |> shouldEqual typeof<Foo>

        attributes.["Case3"]
        |> Attribute.filterRuntime
        |> List.exactlyOne
        |> shouldEqual typeof<Bar>

        attributes.["Case4"]
        |> Attribute.filterRuntime
        |> List.sortBy (fun ty -> ty.Name)
        |> shouldEqual [ typeof<Bar> ; typeof<Foo> ]

    type PrivateFieldUnion =
        private
        | Field1
        | [<Foo>] Field2

    [<Test>]
    let ``Can obtain field data for private-implementation unions`` () =
        let data =
            match tType<PrivateFieldUnion> with
            | Union data -> data
            | _ -> failwith "Unexpected type"

        let fields =
            { new UnionConvEvaluator<PrivateFieldUnion, _> with
                member _.Eval (fields : UnionTypeField list) _ _ = fields
            }
            |> data.Apply

        let attributes =
            fields
            |> List.map (fun field -> field.Name, Attribute.filterFromField field)
            |> Map.ofList

        attributes.Count |> shouldEqual 2
        attributes.["Field1"] |> Attribute.filterRuntime |> shouldBeEmpty

        attributes.["Field2"]
        |> Attribute.filterRuntime
        |> List.exactlyOne
        |> shouldEqual typeof<Foo>
