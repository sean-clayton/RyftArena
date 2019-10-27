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
    type Cooldown = int<s>
    type Health = int
    type Mana = int
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
          ItemPool: Item list
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
          ActorMana: Map<PlayingActor, Mana>
          ActorStatusEffects: Map<PlayingActor, StatusEffect list>
          ActorSkillCooldown: Map<PlayingActor, Cooldown>
          SupportObjectsPositions: Map<PlacedSupportObject, Vector2>
          SupportObjectHealth: Map<PlacedSupportObject, Health> }

    and StatusEffect =
        | Silenced
        | Hexed
        | Stunned

    and Item =
        | MobAttachment of HeroAttachment
        | SupportObject of SupportObject
        | GlobalItem of GlobalItem

    and OwnedItem =
        | OwnedMobAttachment of OwnedHeroAttachment
        | OwnedSupportObject of OwnedSupportObject
        | OwnedGlobalItem of OwnedGlobalItem

    /// TODO:
    /// In the future we should redo this to Common / Rare / Exotic etc
    and ItemTier =
        | One
        | Two
        | Three
        | Four
        | Five

    and HeroAttachment =
        { Id: Guid
          Name: string
          Tier: ItemTier }

    and GlobalItem =
        { Id: Guid
          Name: string
          Tier: ItemTier }

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
        | Underlord of PlacedUnderlord

    and MeleeStyle =
        | Plus
        | Square

    and AttackStyle =
        | Melee of MeleeStyle
        | Ranged of distance: int<u>

    and Movement =
        | Leap
        | Run of speed: int<u/s>

    /// TODO:
    /// Replace these to fit within the lore of the Ryft universe
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
          MaxHealth: Health
          Tier: HeroTier
          AttackStyle: AttackStyle
          Movement: Movement
          Damage: Damage
          Alliances: Alliance list
          Successor: Hero option }

    and Underlord =
        { Id: Guid
          Name: string
          AttackStyle: AttackStyle
          Damage: Damage
          MaxHealth: Health
          MaxMana: Mana }

    and HeroTier =
        | OneStar
        | TwoStar
        | ThreeStar

    and OwnedHero =
        { Id: Guid
          Hero: Hero
          Owner: Player }

    and SupportObject =
        { Id: Guid
          Size: Vector2Int
          Tier: ItemTier }

    and OwnedSupportObject =
        { Id: Guid
          SupportObject: SupportObject
          Owner: Player }

    and OwnedHeroAttachment =
        { Id: Guid
          HeroAttachment: HeroAttachment
          Owner: Player }

    and OwnedUnderlord =
        { Id: Guid
          Underlord: Underlord
          Owner: Player }

    and PlacedUnderlord =
        { Id: Guid
          Underlord: OwnedUnderlord
          PositionOnBoard: Vector2Int }

    and OwnedGlobalItem =
        { Id: Guid
          GlobalItem: GlobalItem
          Owner: Player }

    and PlacedSupportObject =
        { Id: Guid
          PositionOnBoard: Vector2Int
          SupportObject: OwnedSupportObject }

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
          MaxHealth: Health
          AttackStyle: AttackStyle
          Size: Vector2Int
          Movement: Movement }

    and Player =
        { Id: Guid
          PlayerInfo: PlayerInfo
          Underlord: Underlord }

    and PlayerInfo =
        | HumanPlayer of HumanPlayer
        | Bot of BotPlayer

    and BotPlayer =
        { Id: Guid
          BotName: string }

    and HumanPlayer =
        { Id: Guid
          Username: string }
