namespace ShapeSifter.Test

open System
open System.IO
open System.Reflection
open NUnit.Framework
open FsUnitTyped

open CsvParser

[<NoComparison>]
type MyRecord =
    {
        Id : int
        Name : string
        DateOfBirth : DateTime
        NewUser : bool
        Balance : float
    }

[<TestFixture>]
module TestCsvParser =
    let getTestData () : string seq =
        let assembly = Assembly.GetExecutingAssembly ()

        seq {
            use stream =
                assembly.GetManifestResourceNames ()
                |> Seq.filter (fun name -> name.EndsWith ("TestData.csv", StringComparison.Ordinal))
                |> Seq.exactlyOne
                |> assembly.GetManifestResourceStream

            use reader = new StreamReader (stream)
            let mutable isDone = false

            while not isDone do
                let line = reader.ReadLine ()
                if isNull line then isDone <- true else yield line
        }

    [<Test>]
    let ``Example parse`` () =
        let actual =
            getTestData () |> CsvParser.tryParse<MyRecord> |> Option.get |> Seq.toList

        let expected =
            [
                {
                    Id = 1
                    Name = "Derry Williamson"
                    DateOfBirth = DateTime (1974, 03, 12)
                    NewUser = true
                    Balance = 12.34
                }
                {
                    Id = 2
                    Name = "Madelyn Milne"
                    DateOfBirth = DateTime (1988, 11, 23)
                    NewUser = false
                    Balance = 56.78
                }
            ]

        actual |> shouldEqual expected
