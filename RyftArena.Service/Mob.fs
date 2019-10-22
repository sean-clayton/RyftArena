namespace RyftArena.Service

open Common
open Measurements

module Mob =

    type MobHealth = int

    type MobAttachment =
        { Id: Id
          Name: string }

    type MobPlacement =
        | InBench
        | OnBoard

    type MobId = Id

    type MeleeAttackStyle =
        | Plus
        | Square

    type AttackStyle =
        | Ranged of int<M>
        | Melee of MeleeAttackStyle

    type T =
        { Id: MobId
          Name: string
          Tier: int
          Value: int
          MaxHealth: int
          AttackStyle: AttackStyle
          Speed: int<M / Second>
          Successor: T option }

    type MobInPlay =
        { Mob: T
          Owner: Player.T
          Placement: MobPlacement
          Attachment: MobAttachment }
