namespace RyftArena.Utilities
open System.Numerics

module Vector2Int =
    type Vector2Int =
        { X: int
          Y: int }

    let toVector2Float v2Int =
        Vector2(float32 v2Int.X, float32 v2Int.Y)

    let fromVector2Float (v2Float: Vector2) =
        { X = int v2Float.X
          Y = int v2Float.Y }
