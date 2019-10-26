namespace RyftArena

open System
open System.Numerics
open Vector2Int

module Model =
    type RoundNumber = int
    type Health = int
    type Gold = int
    type Damage =
        { Min: int
          Max: int }

    type GameState =
        { Id: Guid
          Players: Player list
          PlayerGold: Map<Player, Gold>
          GameEvents: (GameEvent * DateTime option) list
          HeroPool: Hero list
          Stage: GameStage }

    and GameEvent =
        | InitializeGame
        | InitializeRound
        | HackerDetected of hacker: Player
        | SellHero of OwnedHero
        | ActorAttacked of attacker: PlayingActor * receiver: PlayingActor
        | ActorKilled of PlayingActor
        | NewRound of RoundState
        | RoundStarted of RoundState
        | PlayerReceivedItem of item: Item * receiver: Player
        | PlayerPurchasedHero of newMob: Hero * purchaser: Player
        | PlayerPurchasedExp of xpAmount: int * purchaser: Player
        | PlayerLeveledUp of Player
        | PlayerPlacedAttachmentOnHero of Player * HeroAttachment * OwnedHero
        | RoundEnd of RoundState
        | GameEnded
        | StartedPostGame

    and RoundState =
        { Id: Guid
          RoundNumber: RoundNumber
          HomeOpponent: Opponent
          AwayOpponent: Opponent
          HeroPositions: Map<PlayingActor, Vector2>
          HeroHealth: Map<PlayingActor, Health>
          SupportObjectsPositions: Map<PlacedSupportObject, Vector2> }

    and Item =
        | MobAttachment of HeroAttachment
        | SupportObject of SupportObject
        | GlobalItem of GlobalItem

    and OwnedItem =
        | OwnedMobAttachment of HeroAttachment
        | OwnedSupportObject of SupportObject
        | GlobalItem of GlobalItem

    and HeroAttachment =
        { Id: Guid
          Name: string }

    and GlobalItem =
        { Id: Guid
          Name: string }

    and GameStage =
        | PreGame
        | Playing of currentRound: RoundState * previousRounds: RoundState list
        | PostGame of victor: Player

    and Opponent =
        { Id: Guid
          OpponentType: OpponentType }

    and OpponentType =
        | Player of Player
        | Npc of PlacedMob list

    and PlayingActor =
        | Hero of PlacedHero
        | Mob of PlacedMob

    and MeleeStyle =
        | Plus
        | Square

    and AttackStyle =
        | Melee of MeleeStyle
        | Ranged of distance: int

    and MovementType =
        | Leap
        | Run of speed: int

    and Alliance =
        | Knight
        | Warrior
        | Assassin
        | Dragon
        | Warlock
        | Heartless
        | Savage
        | Hunter
        | Demon
        | Elusive
        | Primordial
        | Human
        | Scaled
        | Troll
        | Scrappy
        | Mage
        | DemonHunter
        | Deadeye
        | Brawny
        | Druid
        | Inventor
        | Shaman
        | BloodBound

    and Hero =
        { Id: Guid
          Rarity: int
          Value: Gold
          Name: string
          Tier: Tier
          AttackStyle: AttackStyle
          Damage: Damage
          Alliances: Alliance list
          Successor: Hero option }

    and Tier =
        | OneStar
        | TwoStar
        | ThreeStar

    and OwnedHero =
        { Id: Guid
          Hero: Hero
          Owner: Player }

    and SupportObject =
        { Id: Guid
          Size: Vector2Int }

    and OwnedSupportObject =
        { Id: Guid
          SupportObject: SupportObject
          Owner: Player }

    and PlacedSupportObject =
        { Id: Guid
          PositionOnBoard: Vector2Int
          SupportObject: SupportObject }

    and PlacedHero =
        { Id: Guid
          Hero: OwnedHero
          PositionOnBoard: Vector2Int }

    and PlacedMob =
        { Id: Guid
          Mob: Mob
          PositionOnBoard: Vector2Int }

    and Mob =
        { Id: Guid
          Damage: Damage
          AttackStyle: AttackStyle
          Size: Vector2Int }

    and Player =
        { Id: Guid
          Username: string
          Heroes: OwnedHero list
          Items: OwnedItem list }
