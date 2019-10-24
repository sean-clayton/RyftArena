module Tests

open System
open RyftArena
open RyftArena.Mob
open RyftArena.Player
open Xunit

module GameTests =
    [<Fact>]
    let ``Creating a game with no players creates a blank state`` () =
        let game = Game.createGame []

        Assert.True (game.GameEvents = [Game.InitializeMatch])
        Assert.True (game.Players = [])

    [<Fact>]
    let ``Creating a game with players has some state`` () =
        let player =
            { Id = Guid.NewGuid()
              Player = Bot
                { Id = Guid.NewGuid()
                  Name = "Cool Bot" } }

        let game = Game.createGame [player]

        Assert.True (game.Players = [player])
        Assert.True (game.PlayerGold = Map [ (player, 1) ])
        Assert.True (game.PlayerHealth = Map [ (player, 100) ])

    [<Fact>]
    let ``Can buy a mob if you have enough money`` () =
        let player =
            { Id = Guid.NewGuid()
              Player = Bot
                { Id = Guid.NewGuid()
                  Name = "Cool Bot" } }

        let game = Game.createGame [player]
        let game =
            { game with
                PlayerGold = Map.add player 100 game.PlayerGold }

        let mob =
                { Id = Guid.NewGuid()
                  Name = "My Mob"
                  Tier = OneStar
                  Value = 1
                  MaxHealth = 800
                  AttackStyle = Melee Square
                  Speed = 5
                  Successor = None }

        match Game.buyMob player mob game with
        | Ok newGame ->
            let mobInPlay =
                newGame.PlayerMobs
                |> Map.find player
                |> List.head

            Assert.True (mobInPlay.Mob = mob)

        | Error _ -> raise (Xunit.Sdk.XunitException "Should not have error")


    [<Fact>]
    let ``Can sell a mob if it's yours`` () =
        let player =
            { Id = Guid.NewGuid()
              Player = Bot
                { Id = Guid.NewGuid()
                  Name = "Cool Bot" } }

        let game = Game.createGame [player]
        let game =
            { game with
                PlayerGold = Map.add player 100 game.PlayerGold }

        let mob =
                { Id = Guid.NewGuid()
                  Name = "My Mob"
                  Tier = OneStar
                  Value = 1
                  MaxHealth = 800
                  AttackStyle = Melee Square
                  Speed = 5
                  Successor = None }

        match Game.buyMob player mob game with
        | Ok game ->
            let mob = game.PlayerMobs |> Map.find player |> List.head

            match Game.sellMob mob game with
            | Ok game ->
                Assert.True (game.PlayerMobs = Map [(player, [])])
                Assert.True (game.PlayerGold = Map [(player, 100)])
            | _ -> raise (Xunit.Sdk.XunitException "Should not have error")

        | _ -> raise (Xunit.Sdk.XunitException "Should not have error")
