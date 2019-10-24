namespace RyftArena

open Common.Errors
open System
open System.Numerics
open Mob

module Game =

    type PlayerHealthAmount = int

    type PlayerGoldAmount = int

    type GameEvent =
        | InitializeMatch
        | HackerDetected of hacker: Player.T
        | SellMob of MobInPlay
        | MobKilled of MobInPlay
        | MobMoved of MobInPlay * Vector2
        | NewRound of Round.T
        | RoundStarted of Round.T
        | BuyMob of newMob: Mob.T * purchaser: Player.T
        | LevelUp of Player.T
        | ReceiveItem of newItem: MobAttachment * receivingPlayer: Player.T

    type Game =
        { Id: Guid
          PlayerGold: Map<Player.T, PlayerGoldAmount>
          Players: Player.T list
          PlayerHealth: Map<Player.T, PlayerHealthAmount>
          PlayerMobs: Map<Player.T, MobInPlay list>
          GameEvents: GameEvent list }

    let buyMob player mob game =
        let previous t = Map.find player t
        let previousGold = previous game.PlayerGold
        let previousMobs = previous game.PlayerMobs

        if previousGold < mob.Value
        then Error (InvalidAction NotEnoughGold)
        else
            let newGoldAmount = previousGold - mob.Value
            let newGoldMap = Map.add player newGoldAmount game.PlayerGold

            let mobInPlay =
                { Id = Guid.NewGuid()
                  Mob = mob
                  Owner = player
                  Placement = InBench
                  Attachment = None }
            let newMobs = mobInPlay :: previousMobs
            let newMobsMap = Map.add player newMobs game.PlayerMobs
            let game =
                { game with
                    PlayerGold = newGoldMap
                    PlayerMobs = newMobsMap
                    GameEvents = BuyMob(mob, player) :: game.GameEvents }

            Ok game

    let sellMob mobInPlay game =
        let previous t = Map.find mobInPlay.Owner t
        let previousGold = previous game.PlayerGold
        let previousMobs = previous game.PlayerMobs
        let newMobs = List.filter ((<>) mobInPlay) previousMobs

        if newMobs = previousMobs
        then Error (InvalidAction InvalidMobSale)
        else Ok { game with
                    PlayerMobs = Map.add mobInPlay.Owner newMobs game.PlayerMobs
                    PlayerGold =
                        game.PlayerGold
                        |> Map.add mobInPlay.Owner (previousGold + mobInPlay.Mob.Value) }

    let createGame players =
        { Id = Guid.NewGuid()
          Players = players
          PlayerGold =
              List.zip players (List.replicate (List.length players) 1)
              |> Map
          PlayerHealth =
              List.zip players (List.replicate (List.length players) 100)
              |> Map
          PlayerMobs =
              List.zip players (List.replicate (List.length players) [])
              |> Map
          GameEvents = [InitializeMatch] }
