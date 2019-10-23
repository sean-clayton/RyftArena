namespace RyftArena

open Common.Errors
open System
open System.Numerics
open Mob

module GameMatch =
    type PlayerHealthAmount = int

    type PlayerGoldAmount = int

    type MatchEvents =
        | InitializeMatch
        | HackerDetected of hacker: Player.T
        | SellMob of MobInPlay
        | MobAttackMob of
            {| round: Round.T
               attacker: MobInPlay
               receiver: MobInPlay
               damageAmount: int |}
        | MobKilled of MobInPlay
        | MobMoved of MobInPlay * Vector2
        | NewRound of Round.T
        | RoundStarted of Round.T
        | BuyMob of newMob: Mob.T * purchaser: Player.T
        | LevelUp of Player.T
        | ReceiveItem of newItem: MobAttachment * receivingPlayer: Player.T

    type T =
        { Id: Guid
          PlayerGold: Map<Player.T, PlayerGoldAmount>
          Players: Player.T list
          PlayerHealth: Map<Player.T, PlayerHealthAmount>
          PlayerMobs: Map<Player.T, MobInPlay list> }

    let sellMob mob game =
        let previousMobs = game.PlayerMobs |> Map.find mob.Owner

        let newMobs = previousMobs |> List.filter ((=) mob)

        match newMobs = previousMobs with
        | true -> Error (InvalidAction InvalidMobSale)
        | _ ->
            Ok { game with
                  PlayerMobs =
                      game.PlayerMobs |> Map.add mob.Owner newMobs
                  PlayerGold =
                      game.PlayerGold
                      |> Map.add mob.Owner
                             ((game.PlayerGold |> Map.find mob.Owner)
                              + mob.Mob.Value) }

    let createGame players =
        { Id = Guid.NewGuid()
          Players = players
          PlayerGold =
              Map.ofList
                  (List.zip players (List.replicate (List.length players) 1))
          PlayerHealth =
              Map.ofList
                  (List.zip players (List.replicate (List.length players) 100))
          PlayerMobs = Map.empty }
