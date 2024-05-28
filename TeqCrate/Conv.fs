namespace TeqCrate

[<NoComparison>]
[<NoEquality>]
type Conv<'a, 'b> =
    {
        To : 'a -> 'b
        From : 'b -> 'a
    }

[<RequireQualifiedAccess>]
module Conv =

    let make (toF : 'a -> 'b) (fromF : 'b -> 'a) =
        {
            To = toF
            From = fromF
        }

    let compose (conv1 : Conv<'a, 'b>) (conv2 : Conv<'b, 'c>) : Conv<'a, 'c> =
        make (conv1.To >> conv2.To) (conv2.From >> conv1.From)
