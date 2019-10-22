namespace RyftArena.Service

module Common =
    module Url =
        type T = Url of string

        let toString = function
            | Url s -> s

        let fromString str = Url str
