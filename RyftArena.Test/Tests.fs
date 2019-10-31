module Tests

open System
open Xunit
open FsUnit.Xunit
open RyftArena
open Model

module GameSessionTests =

    [<Fact>]
    let ``Can create a game session`` () =
        let underlord =
            { Id = Guid.NewGuid()
              Name = "Cool Underlord"
              AttackStyle = Ranged 3<u>
              Damage = 5
              MaxHealth = 1000
              MaxMana = 50 }

        let player =
            { Id = Guid.NewGuid()
              PlayerInfo =
                  Bot
                    { Id = Guid.NewGuid()
                      BotName = "Bot Player" }
              Underlord = underlord }

        let players = [player]
        let gameSession = GameSession.createGameSession players;
        (gameSession.Players = players) |> should be True
