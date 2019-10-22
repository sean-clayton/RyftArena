namespace RyftArena.Service

open Common

module Player =

    type PlayerId = Guid

    type T =
        { Id: PlayerId
          Username: string
          AvatarUrl: Url.T }
