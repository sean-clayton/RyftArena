namespace RyftArena

open System

module Player =

    type UserInfo =
        { Id: Guid
          Username: string
          AvatarUrl: string option }

    type BotInfo =
        { Id: Guid
          Name: string }

    type Player =
        | Human of UserInfo
        | Bot of BotInfo

    type T =
        { Id: Guid
          Player: Player }

    let createPlayer player =
        { Id = Guid.NewGuid()
          Player = player }

    let createHumanPlayer userName avatarUrl =
        { Id = Guid.NewGuid()
          Username = userName
          AvatarUrl = avatarUrl }
        |> Human
        |> createPlayer

    let createBotPlayer name =
        { Id = Guid.NewGuid(); Name = name }
        |> Bot
        |> createPlayer
