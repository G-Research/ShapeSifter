namespace TeqCrate

open System
open TeqCrate.TypePatterns

[<RequireQualifiedAccess>]
module Type =

    let rec print (t : Type) =
        match t with
        | _ when t = typeof<int> -> "int"
        | _ when t = typeof<int64> -> "int64"
        | _ when t = typeof<float> -> "float"
        | _ when t = typeof<char> -> "char"
        | _ when t = typeof<string> -> "string"
        | _ when t = typeof<bool> -> "bool"
        | Fun (domain, range) -> sprintf "(%s -> %s)" (print domain) (print range)
        | Tuple ts -> ts |> Seq.map print |> String.concat " * " |> sprintf "(%s)"
        | Array e -> sprintf "%s array" (print e)
        | Generic (t, ts) ->

            let t =
                match t with
                | _ when t = typedefof<_ list> -> "list"
                | _ when t = typedefof<_ seq> -> "seq"
                | _ when t = typedefof<Map<_, _>> -> "Map"
                | _ -> t.Name

            match ts with
            | [ e ] -> sprintf "%s %s" (print e) t
            | _ -> sprintf "%s<%s>" t (ts |> Seq.map print |> String.concat ", ")

        | _ -> t.Name
