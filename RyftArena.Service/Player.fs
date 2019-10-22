namespace RyftArena.Service

open Common

module Player =

    type PlayerId = Id

    type T =
        { Id: PlayerId
          Username: string
          AvatarUrl: Url.T }
