namespace TeqCrate

open System
open TypeEquality

module Patterns =

    type 'a TType = | TType of unit

    let tType<'a> : 'a TType = TType ()

    let (|Bool|_|) (_ : 'a TType) : Teq<'a, bool> option = Teq.tryRefl<'a, bool>

    let (|Int|_|) (_ : 'a TType) : Teq<'a, int> option = Teq.tryRefl<'a, int>

    let (|Int64|_|) (_ : 'a TType) : Teq<'a, int64> option = Teq.tryRefl<'a, int64>

    let (|Float|_|) (_ : 'a TType) : Teq<'a, float> option = Teq.tryRefl<'a, float>

    let (|String|_|) (_ : 'a TType) : Teq<'a, string> option = Teq.tryRefl<'a, string>

    let (|Unit|_|) (_ : 'a TType) : Teq<'a, unit> option = Teq.tryRefl<'a, unit>

    let (|DateTime|_|) (_ : 'a TType) : Teq<'a, DateTime> option = Teq.tryRefl<'a, DateTime>

    let (|TimeSpan|_|) (_ : 'a TType) : Teq<'a, TimeSpan> option = Teq.tryRefl<'a, TimeSpan>

    let (|Teq|_|) (_ : 'b TType) (_ : 'a TType) : Teq<'a, 'b> option = Teq.tryRefl<'a, 'b>

    let (|Array|_|) (_ : 'a TType) : 'a ArrayTeqCrate option = ArrayTeqCrate.tryMake ()

    let (|List|_|) (_ : 'a TType) : 'a ListTeqCrate option = ListTeqCrate.tryMake ()

    let (|Seq|_|) (_ : 'a TType) : 'a SeqTeqCrate option = SeqTeqCrate.tryMake ()

    let (|Option|_|) (_ : 'a TType) : 'a OptionTeqCrate option = OptionTeqCrate.tryMake ()

    let (|Set|_|) (_ : 'a TType) : 'a SetTeqCrate option = SetTeqCrate.tryMake ()

    let (|Map|_|) (_ : 'a TType) : 'a MapTeqCrate option = MapTeqCrate.tryMake ()

    let (|Dictionary|_|) (_ : 'a TType) : 'a DictionaryTeqCrate option = DictionaryTeqCrate.tryMake ()

    let (|ResizeArray|_|) (_ : 'a TType) : 'a ResizeArrayTeqCrate option = ResizeArrayTeqCrate.tryMake ()

    let (|Fun|_|) (_ : 'a TType) : 'a FunTeqCrate option = FunTeqCrate.tryMake ()

    let (|Pair|_|) (_ : 'a TType) : 'a PairTeqCrate option = PairTeqCrate.tryMake ()

    let (|Triple|_|) (_ : 'a TType) : 'a TripleTeqCrate option = TripleTeqCrate.tryMake ()

    let (|Tuple|_|) (_ : 'a TType) : 'a TupleConvCrate option = TupleConvCrate.tryMake ()

    let (|Record|_|) (_ : 'a TType) : 'a RecordConvCrate option = RecordConvCrate.tryMake ()

    let (|Union|_|) (_ : 'a TType) : 'a UnionConvCrate option = UnionConvCrate.tryMake ()

    let (|SumOfProducts|_|) (_ : 'a TType) : 'a SumOfProductsConvCrate option = SumOfProductsConvCrate.tryMake ()
