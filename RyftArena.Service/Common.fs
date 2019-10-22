namespace RyftArena.Service

module Common =

    type Id = Id of string

    let generateId() = Id(System.Guid.NewGuid().ToString())

    module Measurements =
        [<MeasureAttribute>]
        type M

        [<MeasureAttribute>]
        type Second

    module Url =
        type T = Url of string

        let toString = function
            | Url s -> s

        let fromString str = Url str

    module TwoDimensional =
        type Position =
            { X: int
              Y: int }

        /// <summary>
        /// Gets the distance between two Positions
        /// </summary>
        let distance p1 p2 = sqrt (float (pown (p2.X - p1.X) 2) + float (pown (p2.Y - p1.Y) 2))
