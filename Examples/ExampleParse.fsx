// Note: run 'dotnet publish' in the root of this repo to ensure that the dlls referenced in this script are present

#load "CsvParser.fsx"

open System
open System.IO

type MyRecord =
    {
        Id : int
        Name : string
        DateOfBirth : DateTime
        NewUser : bool
        Balance : float
    }

Path.Combine (__SOURCE_DIRECTORY__, "TestData.csv")
|> FileInfo
|> CsvParser.tryParse<MyRecord>
|> Option.get
|> Seq.iter (printfn "%A")
