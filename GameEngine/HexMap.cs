using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using System;

namespace WindowsGame1
{
    public class HexMap
    {
        private readonly List<Hex> hexes;
        private readonly Hex[,] indexedHexes;

        private HexMap(IEnumerable<Hex> hexes)
        {
            this.hexes = new List<Hex>(hexes);

            var upperLeft = new Point(this.Hexes.Min(x => x.Position.X), this.Hexes.Min(x => x.Position.Y));
            var lowerRight = new Point(this.Hexes.Max(x => x.Position.X), this.Hexes.Max(x => x.Position.Y));
            this.Area = new Rectangle(upperLeft.X, upperLeft.Y, lowerRight.X - upperLeft.X + 1, lowerRight.Y - upperLeft.Y + 1);

            this.indexedHexes = new Hex[this.Area.Width, this.Area.Height];
            foreach (var hex in this.hexes)
            {
                this.indexedHexes[hex.Position.X, hex.Position.Y] = hex;
            }
        }

        public IEnumerable<Hex> Hexes
        {
            get { return this.hexes; }
        }

        public Rectangle Area { get; private set; }

        public Hex this[int x, int y]
        {
            get { return this.indexedHexes[x, y]; }
        }

        public float HexRadius
        {
            get { return this.hexes.First().Radius; }
        }

        public float HexEdgeLength
        {
            get { return this.hexes.First().EdgeLength; }
        }

        public static int HexDistance(Hex first, Hex second)
        {
            var deltaX = second.Position.X - first.Position.X;
            var deltaY = second.Position.Y - first.Position.Y;

            //if ((deltaX >= 0 && deltaY >= 0) || (deltaX < 0 && deltaY < 0))
            //{ // sign(deltaX) == sign(deltaY)
            //    return Math.Max(Math.Abs(deltaX), Math.Abs(deltaY));
            //}
            //else
            //{
            //    return Math.Abs(deltaX) + Math.Abs(deltaY);
            //}

            //return Math.Max(Math.Abs(deltaX), Math.Abs(deltaY));

            //if (deltaX == 0 || deltaY == 0)
            //    return Math.Abs(deltaX) + Math.Abs(deltaY);

            //if (deltaY < 0)
            //    return Math.Abs(deltaX) + Math.Abs(deltaY) - 1;

            //
            //var extra = Math.Abs(deltaY) - (Math.Abs(deltaX) / 2);
            //return Math.Abs(deltaX) + (extra > 0 ? extra : 0); 

            //var adx = Math.Abs(deltaX);
            //var ady = Math.Abs(deltaY) * 2 + adx % 2;
            //return Math.Max(adx, (int)Math.Ceiling(ady / 2m));

            var adj = first.Position.X % 2 == 0 ? 0.5 : -0.5;
            var adx = Math.Abs(deltaX);
            var ady = Math.Abs(deltaY + (adx % 2 * adj));

            if (adx >= 2 * ady) return adx;
            return (int)(adx + ady - adx / 2.0);

            //if (adx > ady)
            //    return adx;
            //else
            //    return adx + (int)Math.Floor(ady) - adx;


            //if (deltaY < 0)
            //    return 2 * Math.Abs(deltaX) - Math.Abs(deltaY);

            //return (Math.Abs(deltaX) + Math.Abs(deltaY) + Math.Abs(deltaX + deltaY)) / 2;
            return 0;
        }

        //public static IEnumerable<Hex> CreateHexMap(float edgeLength, float area)
        public static HexMap CreateHexMap(float edgeLength, float area)
        {
            var temp = new Hex(Vector2.Zero, edgeLength);
            //return CreateHexMap(new Vector2(temp.Width / 2f, temp.Height / 2f), edgeLength, area);
            //return CreateHexMap(new Vector2(temp.Width / 2f, temp.Height / 2f), edgeLength, new Rectangle(0, 0, (int)area, (int)area));
            return new HexMap(CreateHexMap(
                new Vector2(temp.Width / 2f, temp.Height / 2f),
                edgeLength,
                new Rectangle(0, 0, (int)area, (int)area)));
        }

        //public static IEnumerable<Hex> CreateHexMap(Vector2 startingCenter, float edgeLength, float area)

        public static IEnumerable<Hex> CreateHexMap(Vector2 startingCenter, float edgeLength, Rectangle areaRectangle)
        {
            //var areaRectangle = new Rectangle(0, 0, (int)area, (int)area);
            //var centerDistance = (float)Math.Sqrt(Math.Pow(edgeLength * 2, 2) - Math.Pow(edgeLength, 2));
            //var centerDistance = (float)Math.Sqrt((edgeLength * 2).Square() - edgeLength.Square());
            //var hexDistance = ((edgeLength * 2).Square() - edgeLength.Square()).SquareRoot();
            var hexRadius = MathUtil.CalcHypotenuseSide(edgeLength * 2, edgeLength);
            //    return GetAdjacentHexes(new Vector2(1, 1), centerDistance, area).Select(x => new Hex(x, edgeLength));
            //}

            //private static IEnumerable<Vector2> GetAdjacentHexes(Vector2 startingCenter, float hexDistance, float area)
            //{
            var angles = new[] { -30, 30, 90 };
            var hexCenters = new HashSet<Hex>();
            //var toExplore = new HashSet<Vector2> { startingCenter };
            var toExplore = new HashSet<Hex> { new Hex(startingCenter, edgeLength, areaRectangle.Location) };

            while (toExplore.Any())
            {
                var currentHex = toExplore.First();
                toExplore.Remove(currentHex);

                foreach (var angle in angles)
                {
                    var position = Point.Zero;
                    switch (angle)
                    {
                        case -30: position = currentHex.Position.Translate(new Point(1, currentHex.Position.X % 2 == 0 ? -1 : 0)); break;
                        case 30: position = currentHex.Position.Translate(new Point(1, currentHex.Position.X % 2 == 0 ? 0 : 1)); break;
                        case 90: position = currentHex.Position.Translate(new Point(0, 1)); break;
                    }
                    //var adjacent = GetAdjacentHex(hex, angle, hexDistance);
                    var adjacent = currentHex.Center
                        .TranslatePolar(MathHelper.ToRadians(angle), hexRadius)
                        .RoundTo(1);

                    var adjacentHex = new Hex(adjacent, edgeLength, position);
                    //if (adjacent.X >= 0 && adjacent.Y >= 0 && adjacent.X <= area && adjacent.Y <= area)
                    //if (areaRectangle.Contains(adjacent.ToPoint()))
                    if (areaRectangle.Contains(adjacentHex.Position))
                    {
                        if (!toExplore.Contains(adjacentHex) && !hexCenters.Contains(adjacentHex))
                            toExplore.Add(adjacentHex);
                    }
                }

                hexCenters.Add(currentHex);
            }

            return hexCenters;
        }
    }
}
