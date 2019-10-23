namespace RyftArena

module Common =
    module Errors =
        type InvalidActions =
            | InvalidMobUpgrade
            | InvalidMobSale

        type RyftArenaError =
            | InvalidAction of InvalidActions
