namespace RyftArena.Service

open System

module Mob =
    type MobHealth = int

    type MobAttachment =
        { Id: Guid
          Name: string }

    type MobPlacement =
        | InBench
        | OnBoard

    type MeleeAttackStyle =
        | Plus
        | Square

    type AttackStyle =
        | Ranged of int
        | Melee of MeleeAttackStyle

    type T =
        { Id: Guid
          Name: string
          Tier: int
          Value: int
          MaxHealth: int
          AttackStyle: AttackStyle
          Speed: int
          Successor: T option }

    type MobInPlay =
        { Id: Guid
          Mob: T
          Owner: Player.T
          Placement: MobPlacement
          Attachment: MobAttachment }
