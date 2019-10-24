namespace RyftArena

module Common =
    module Errors =
        type InvalidActions =
            | InvalidMobUpgrade
            | InvalidMobSale
            | NotEnoughGold

        type RyftArenaError = InvalidAction of InvalidActions
