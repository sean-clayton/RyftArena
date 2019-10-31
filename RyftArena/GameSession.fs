namespace RyftArena

open System
open Model

module GameSession =
    let createGameSession players =
        let now = DateTime.UtcNow

        { Id = Guid.NewGuid()
          Players = players
          ConnectedPlayers = players
          DisconnectedPlayers = []
          PlayerGold = Map.empty
          PlayerHeroes = Map.empty
          PlayerItems = Map.empty
          GameEvents = [InitializeGame, Some now]
          HeroPool = []
          ItemPool = []
          Stage = PreGame }
