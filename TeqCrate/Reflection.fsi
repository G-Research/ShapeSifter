namespace TeqCrate

open Microsoft.FSharp.Quotations
open System

/// Utility module that provides helper functions for making reflective calls easy.
[<RequireQualifiedAccess>]
module Reflection =

    /// Given a quotation whose body is just a call to a static method (for example
    /// a module method), invokes the method with the supplied sequence of types
    /// as the generic type parameters and the supplied sequence of objects as
    /// the parameters to the method.
    /// Returns the boxed return value of the method.
    val invokeStaticMethod : Expr -> (Type seq -> obj seq -> obj)
