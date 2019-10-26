namespace RyftArena

open System
open System.Numerics
open Utilities.Vector2Int

module Model =
    /// Time, seconds
    [<Measure>] type s
    /// Distance, generic unit
    [<Measure>] type u

    type RoundNumber = int
    type Health = int
    type Gold = int
    type Exp = int
    type Damage = int

    type GameState =
        { Id: Guid
          Players: Player list
          ConnectedPlayers: Player list
          DisconnectedPlayers: Player list
          PlayerGold: Map<Player, Gold>
          PlayerHeroes: Map<Player, Hero list>
          PlayerItems: Map<Player, Item list>
          GameEvents: (GameEvent * DateTime option) list
          HeroPool: Hero list
          Stage: GameStage }

    and GameEvent =
        | InitializeGame
        | InitializeRound
        | CheaterDetected of HumanPlayer
        | SellHero of OwnedHero
        | ActorAttacked of attacker: PlayingActor * receiver: PlayingActor
        | ActorKilled of PlayingActor
        | NewRound of RoundState
        | RoundStarted of RoundState
        | PlayerReceivedItem of item: Item * receiver: Player
        | PlayerPurchasedHero of newHero: Hero * purchaser: Player
        | PlayerPurchasedExp of Exp * purchaser: Player
        | PlayerLeveledUp of Player
        | PlayerPlacedAttachmentOnHero of Player * HeroAttachment * OwnedHero
        | RoundEnd of RoundState
        | GameEnded
        | StartedPostGame

    and RoundState =
        { Id: Guid
          TimeLeftInStage: int<s>
          RoundNumber: RoundNumber
          HomeOpponent: Opponent
          AwayOpponent: Opponent
          ActorPositions: Map<PlayingActor, Vector2>
          ActorHealth: Map<PlayingActor, Health>
          SupportObjectsPositions: Map<PlacedSupportObject, Vector2>
          SupportObjectHealth: Map<PlacedSupportObject, Health> }

    and Item =
        | MobAttachment of HeroAttachment
        | SupportObject of SupportObject
        | GlobalItem of GlobalItem

    and OwnedItem =
        | OwnedMobAttachment of OwnedHeroAttachment
        | OwnedSupportObject of OwnedSupportObject
        | GlobalItem of OwnedGlobalItem

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
        | Ranged of distance: int<u>

    and Movement =
        | Leap
        | Run of speed: int<u/s>

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
          Movement: Movement
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

    and OwnedHeroAttachment =
        { Id: Guid
          HeroAttachment: HeroAttachment
          Owner: Player }

    and OwnedGlobalItem =
        { Id: Guid
          GlobalItem: GlobalItem
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
          Size: Vector2Int
          Movement: Movement }

    and PlayerInfo =
        | HumanPlayer of HumanPlayer
        | Bot of BotPlayer

    and BotPlayer =
        { Id: Guid
          BotName: string }

    and HumanPlayer =
        { Id: Guid
          Username: string }

    and Player =
        { Id: Guid
          PlayerInfo: PlayerInfo }
