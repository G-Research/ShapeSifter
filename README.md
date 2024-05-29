# ShapeSifter

Type-safe datatype-generic programming for F#.

## Getting started

The most useful place to start is likely with the `tType<'a>` function and corresponding active patterns in the `Patterns` module.
These patterns reflectively determine what type `'a` was, and give you evidence in the form of a `Teq` (see [TypeEquality](https://github.com/G-Research/TypeEquality)).
Where appropriate, you also get a type-safe representation of the type's structure.

Here is a brief example.
Everything written here was forced by the types: once we chose to match on the `Unit` and `Record` active patterns, there was only one way to write this function so that it compiled.

```fsharp
open ShapeSifter
open ShapeSifter.Patterns

let manipulateType<'a> () =
    match tType<'a> with
    | Unit teq ->
        printfn "'a was a unit type, and `teq` witnesses this!"
    | Record data ->
        { new RecordConvEvaluator<_> with
            member _.Eval (fieldData : RecordTypeField list) (fieldTypes : TypeList<'ts>) (conv : Conv<'a, 'ts HList>) =
                failwith "manipulate the type here"
        }
        |> data.Apply
    | _ -> failwith "unrecognised type"
```

Inside the `RecordConvEvaluator`, we have gained access to:

* The list `fieldData` of record fields, telling us the name of each field and any attributes that were on the field (as well as the raw `PropertyInfo` associated with each field).
* The same list of field types, but expressed as a [`HeterogeneousCollections.TypeList`](https://github.com/G-Research/HeterogeneousCollections/blob/main/HeterogeneousCollections/TypeList.fsi).
* A `Conv` (converter) which lets us interchange between an `'a` and a heterogeneous list of its field values.

We have now seen a pattern for a primitive type (`Unit`) and for an arbitrary record.
Using the patterns in ShapeSifter, we can recognise the following types:

* Many primitive types, and `DateTime` and `TimeSpan`
* `Array<_>`, `_ list`, `Seq<_>`, `Set<_>`
* `Option<_>`
* `Map<_, _>`
* `_ * _`, `_ * _ * _`, and arbitrary tuples
* `_ -> _`
* `Dictionary<_,_>`, `ResizeArray<_>`
* `Teq<_, _>`
* Records and unions
* "Sums of products" (that is, unions, but where we give you easier access to the products which make up the union fields).

## More examples

See the [tests](./ShapeSifter.Test) for examples demonstrating how to perform type-safe manipulation of various different types.
There is a [whistlestop tour](./ShapeSifter.Test/TestExamples.fs) and a [specific example of type-safe CSV parsing](./ShapeSifter.Test/CsvExample).

## Credits

This library was originally built by [Nicholas Cowle](https://github.com/nickcowle).
