namespace RyftArena

open System
open System.Numerics
open Mob

module Round =

    type RoundResult =
        | Victor of Player.T
        | Draw

    type Stage =
        | Draft
        | Preparing
        | Combat
        | Resolving
        | Looting

    type RoundKind =
        | Pve
        | Pvp
        | PvpWithDraft
        | PveWithDraft

    type T =
        { Id: Guid
          Name: int
          Stage: Stage
          OpponentA: Player.T
          OpponentB: Player.T
          RoundKind: RoundKind
          mutable MobPositions: Map<MobInPlay, Vector2>
          mutable MobHealth: Map<MobInPlay, MobHealth> }

    let createRound opponentA opponentAMobs opponentB opponentBMobs =
        let mobs = opponentAMobs @ opponentBMobs
        let mobPositions =
            List.zip mobs (List.replicate (List.length mobs) Vector2.Zero)
            |> Map
        let mobHealth =
            List.zip mobs (List.replicate (List.length mobs) 100)
            |> Map

        { Id = Guid.NewGuid()
          Name = 1
          Stage = Draft
          OpponentA = opponentA
          OpponentB = opponentB
          RoundKind = PveWithDraft
          MobPositions = mobPositions
          MobHealth = mobHealth }

    /// <summary>
    /// Gets the winner from a round,
    /// but only if the round is in the resolving stage.
    /// Otherwise, you get a `None`
    /// </summary>
    let getWinnerFromRound round =
        match round.Stage with
        | Resolving ->
            let remainingMobs =
                round.MobHealth |> Map.filter (fun _mob health -> health > 0)

            let opponentIsAlive opponent =
                remainingMobs
                |> Map.exists (fun mob _health -> mob.Owner = opponent)

            let opponentAIsAlive = opponentIsAlive round.OpponentA
            let opponentBisAlive = opponentIsAlive round.OpponentB

            match opponentAIsAlive, opponentBisAlive with
            | true, _ -> Some(Victor round.OpponentA)
            | _, true -> Some(Victor round.OpponentB)
            | _ -> Some Draw

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

    let moveMob mob newPosition round =
        round.MobPositions <-
            round.MobPositions
            |> Map.map (fun m position ->
                match m with
                | m when m = mob -> newPosition
                | _ -> position)

    /// <summary>
    /// Determines if a given mob is an enemy to a given player
    /// </summary>
    let mobIsEnemyToPlayer player mob = mob.Owner <> player

    let getNewRound previousRound =
        let pveWithDraft = { previousRound with RoundKind = PveWithDraft }
        let pve = { previousRound with RoundKind = Pve }
        let pvpWithDraft = { previousRound with RoundKind = PvpWithDraft }
        let pvp = { previousRound with RoundKind = Pvp }

        let newRound =
            match previousRound.Name with
            | 2
            | 15
            | 20 -> pve
            | 14 | 21 -> pvpWithDraft
            | num when num % 5 = 0 -> pveWithDraft
            | _ -> pvp

        { newRound with Id = Guid.NewGuid() }

    let getRoundTime round =
        match round.Stage with
        | Draft -> 30
        | Preparing -> 25
        | Combat -> 60
        | Resolving -> 30
        | Looting -> 15
