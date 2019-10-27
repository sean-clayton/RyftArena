module Tests

open Xunit
open FsUnit.Xunit

[<Fact>]
let ``true is true``() =
    true |> should equal true

