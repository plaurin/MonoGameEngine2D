using System;
using System.Collections.Generic;
using System.Linq;

namespace ClassLibrary.Hexes
{
    public class HexGrid
    {
        private readonly List<HexGridElement> hexes;
        private readonly HexGridElement[,] indexedHexes;

        private HexGrid(IEnumerable<HexGridElement> hexes)
        {
            this.hexes = new List<HexGridElement>(hexes);

            var upperLeft = new Point(this.Hexes.Min(x => x.Position.X), this.Hexes.Min(x => x.Position.Y));
            var lowerRight = new Point(this.Hexes.Max(x => x.Position.X), this.Hexes.Max(x => x.Position.Y));
            this.Area = new Rectangle(upperLeft.X, upperLeft.Y, lowerRight.X - upperLeft.X + 1, lowerRight.Y - upperLeft.Y + 1);

            this.indexedHexes = new HexGridElement[this.Area.Width, this.Area.Height];
            foreach (var hex in this.hexes)
            {
                this.indexedHexes[hex.Position.X, hex.Position.Y] = hex;
            }
        }

        public IEnumerable<HexGridElement> Hexes
        {
            get { return this.hexes; }
        }

        public Rectangle Area { get; private set; }

        public HexGridElement this[int x, int y]
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

        public static int HexDistance(HexGridElement first, HexGridElement second)
        {
            var deltaX = second.Position.X - first.Position.X;
            var deltaY = second.Position.Y - first.Position.Y;

            var adj = first.Position.X % 2 == 0 ? 0.5 : -0.5;
            var adx = Math.Abs(deltaX);
            var ady = Math.Abs(deltaY + (adx % 2 * adj));

            if (adx >= 2 * ady) return adx;
            return (int)(adx + ady - adx / 2.0);
        }

        public static HexGrid CreateHexMap(float edgeLength, float area)
        {
            var temp = new HexGridElement(Vector.Zero, edgeLength);

            return new HexGrid(CreateHexMap(
                new Vector(temp.Width / 2f, temp.Height / 2f),
                edgeLength,
                new Rectangle(0, 0, (int)area, (int)area)));
        }

        public static IEnumerable<HexGridElement> CreateHexMap(Vector startingCenter, float edgeLength, Rectangle areaRectangle)
        {
            var hexRadius = MathUtil.CalcHypotenuseSide(edgeLength * 2, edgeLength);
            var angles = new[] { -30, 30, 90 };
            var hexCenters = new HashSet<HexGridElement>();
            var toExplore = new HashSet<HexGridElement> { new HexGridElement(startingCenter, edgeLength, areaRectangle.Location) };

            while (toExplore.Any())
            {
                var currentHex = toExplore.First();
                toExplore.Remove(currentHex);

                foreach (var angle in angles)
                {
                    var position = Point.Zero;
                    switch (angle)
                    {
                        case -30: 
                            position = currentHex.Position.Translate(new Point(1, currentHex.Position.X % 2 == 0 ? -1 : 0)); 
                            break;
                        case 30: 
                            position = currentHex.Position.Translate(new Point(1, currentHex.Position.X % 2 == 0 ? 0 : 1)); 
                            break;
                        case 90: 
                            position = currentHex.Position.Translate(new Point(0, 1)); 
                            break;
                    }

                    var adjacent = currentHex.Center
                        .TranslatePolar(MathUtil.ToRadians(angle), hexRadius)
                        .RoundTo(1);

                    var adjacentHex = new HexGridElement(adjacent, edgeLength, position);
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
