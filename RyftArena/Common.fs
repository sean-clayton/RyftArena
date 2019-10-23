namespace RyftArena

module Common =

    module Errors =
        type InvalidActions =
            | InvalidMobUpgrade

        type RyftArenaError =
            | InvalidAction of InvalidActions
