namespace RyftArena

open System
open Common.Errors

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

    type Tier =
        | OneStar
        | TwoStar
        | ThreeStar

    type T =
        { Id: Guid
          Name: string
          Tier: Tier
          Value: int
          MaxHealth: int
          AttackStyle: AttackStyle
          Speed: int
          Successor: T option }

    let getNextTier mob =
        match mob with
        | { Tier = OneStar } -> Ok { Option.get mob.Successor with Tier = TwoStar }
        | { Tier = TwoStar } -> Ok { Option.get mob.Successor with Tier = ThreeStar }
        | _ -> Error(InvalidAction InvalidMobUpgrade)

    type MobInPlay =
        { Id: Guid
          Mob: T
          Owner: Player.T
          Placement: MobPlacement
          Attachment: MobAttachment option }
