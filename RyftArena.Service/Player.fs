namespace RyftArena.Service

open Common

module Player =

    type PlayerId = Id.T

    type T =
        { Id: PlayerId
          Username: string
          AvatarUrl: Url.T }
