namespace RyftArena.Service

open Common
open Mob

module Round =

    type RoundResult =
        | Victor of Player.T
        | Draw

    type Stage =
        | Preparing
        | Combat
        | Resolving
        | Looting

    type RoundKind =
        | Neutral
        | PvP

    type T =
        { Name: int
          Stage: Stage
          OpponentA: Player.T
          OpponentB: Player.T
          RoundKind: RoundKind
          mutable MobPositions: Map<MobInPlay, TwoDimensional.Position>
          mutable MobHealth: Map<MobInPlay, MobHealth> }


    /// <summary>
    /// Gets the winner from a round,
    /// but only if the round is in the resolving stage.
    /// Otherwise, you get a `None`
    /// </summary>
    let getWinnerFromRound round =
        match round.Stage with
        | Resolving ->
            let remainingMobs = round.MobHealth |> Map.filter (fun _mob health -> health > 0)
            let opponentAIsAlive = remainingMobs |> Map.exists (fun mob _health -> mob.Owner = round.OpponentA)
            let opponentBisAlive = remainingMobs |> Map.exists (fun mob _health -> mob.Owner = round.OpponentB)

            match (opponentAIsAlive, opponentBisAlive) with
            | (true, _) -> Some(Victor round.OpponentA)
            | (_, true) -> Some(Victor round.OpponentB)
            | (false, false) -> Some Draw

        | _ -> None

    /// <summary>
    /// Mutates the round's `MobHealth` map to a new map
    /// containing a damaged mob
    /// </summary>
    let mobTakeDamage receiver amount round =
        round.MobHealth <-
            round.MobHealth
            |> Map.map (fun mob health ->
                match mob with
                | m when m = receiver ->
                    let remainingHealth = health - amount
                    if (remainingHealth <= 0) then 0
                    else remainingHealth
                | _ -> health)

    /// <summary>
    /// Determines if a given mob is an enemy to a given player
    /// </summary>
    let mobIsEnemyToPlayer player mob = mob.Owner <> player

    let getNewRound previousRound =
        match previousRound.Name with
        | 1
        | 2
        | 15
        | 20 -> { previousRound with RoundKind = Neutral }
        | _ -> { previousRound with RoundKind = PvP }

    let getRoundTime round =
        match round.Stage with
        | Preparing -> 25
        | Combat -> 60
        | Resolving -> 30
        | Looting -> 15
