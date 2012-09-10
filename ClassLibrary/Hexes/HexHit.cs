using System;

using ClassLibrary.Scenes;

namespace ClassLibrary.Hexes
{
    public class HexHit : HitBase
    {
        public HexHit(Point point)
        {
            this.Point = point;
        }

        public Point Point { get; private set; }

        public override string ToString()
        {
            return "Hex: " + this.Point;
        }
    }
}