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

    let createBot name =
        { Id = Guid.NewGuid()
          Player = Bot { Id = Guid.NewGuid(); Name = name } }
