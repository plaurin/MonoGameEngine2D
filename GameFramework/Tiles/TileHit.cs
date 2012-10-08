using System;

using GameFramework.Scenes;

namespace GameFramework.Tiles
{
    public class TileHit : HitBase
    {
        public TileHit(Point point)
        {
            this.Point = point;
        }

        public Point Point { get; private set; }

        public override string ToString()
        {
            return "Tile: " + this.Point;
        }
    }
}