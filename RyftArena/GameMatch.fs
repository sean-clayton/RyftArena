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
        let previous t = Map.find mob.Owner t
        let previousGold = previous game.PlayerGold
        let previousMobs = previous game.PlayerMobs
        let newMobs = List.filter ((<>) mob) previousMobs

        if newMobs = previousMobs
        then Error (InvalidAction InvalidMobSale)
        else Ok { game with
                    PlayerMobs = Map.add mob.Owner newMobs game.PlayerMobs
                    PlayerGold = game.PlayerGold
                        |> Map.add mob.Owner (previousGold + mob.Mob.Value) }

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
