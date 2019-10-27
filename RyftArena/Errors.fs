namespace RyftArena

module Errors =
    module Errors =
        type InvalidActions =
            | InvalidMobUpgrade
            | InvalidMobSale
            | NotEnoughGold

        type RyftArenaError = InvalidAction of InvalidActions
