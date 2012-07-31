using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace WindowsGame1
{
    public class Hex
    {
        public Hex(Vector2 center, float edgeLength)
            : this(center, edgeLength, Point.Zero)
        {
        }

        public Hex(Vector2 center, float edgeLength, Point position)
        {
            this.EdgeLength = edgeLength;
            this.Center = center;
            this.Position = position;

            this.Width = 2 * this.EdgeLength;
            //this.height = (float)Math.Pow(Math.Pow(this.EdgeLength, 2) - Math.Pow(this.EdgeLength / 2.0f, 2), 0.5);
            //this.height = (float)Math.Pow(this.EdgeLength.Square() - (this.EdgeLength / 2.0f).Square(), 0.5);
            //this.height = (this.EdgeLength.Square() - (this.EdgeLength / 2.0f).Square()).SquareRoot();
            this.Height = 2 * MathUtil.CalcHypotenuseSide(this.EdgeLength, this.EdgeLength / 2.0f);
            this.Radius = MathUtil.CalcHypotenuseSide(this.EdgeLength * 2, this.EdgeLength);
        }

        public Vector2 Center { get; private set; }

        public float EdgeLength { get; private set; }

        public Point Position { get; private set; }

        public float Width { get; private set; }

        public float Height { get; private set; }

        public float Radius { get; private set; }

        public IEnumerable<Vector2> GetVertices()
        {
            //for (var i = 0; i < 6; i++)
            //{
            //    //var angle = MathHelper.ToRadians(60 + (i * 60));
            //    //yield return this.Center + new Vector2((float)Math.Cos(angle) * this.EdgeLength, (float)Math.Sin(angle) * this.EdgeLength);
            //    yield return this.Center.TranslatePolar(MathHelper.ToRadians(60 + (i * 60)), this.EdgeLength);
            //}
            return Enumerable.Range(0, 6)
                .Select((x, i) => this.Center.TranslatePolar(MathHelper.ToRadians(60 + (i * 60)), this.EdgeLength));
        }

        //public static IEnumerable<Hex> CreateHexMapOld(float hexSide, float area)
        //{
        //    //var centerDistance = (float)Math.Sqrt(Math.Pow(edgeLength * 2, 2) - Math.Pow(edgeLength, 2));
        //    //var centerDistance = (float)Math.Sqrt((edgeLength * 2).Square() - edgeLength.Square());
        //    //var hexDistance = ((edgeLength * 2).Square() - edgeLength.Square()).SquareRoot();
        //    var hexDistance = MathUtil.CalcHypotenuseSide(hexSide * 2, hexSide);
        //    //    return GetAdjacentHexes(new Vector2(1, 1), centerDistance, area).Select(x => new Hex(x, edgeLength));
        //    //}

        //    //private static IEnumerable<Vector2> GetAdjacentHexes(Vector2 startingCenter, float hexDistance, float area)
        //    //{
        //    var angles = new[] { -30, 30, 90 };
        //    var hexCenters = new HashSet<Vector2>();
        //    //var toExplore = new HashSet<Vector2> { startingCenter };
        //    var toExplore = new HashSet<Vector2> { new Vector2(1, 1) };

        //    while (toExplore.Any())
        //    {
        //        var currentHex = toExplore.First();
        //        toExplore.Remove(currentHex);

        //        foreach (var angle in angles)
        //        {
        //            //var adjacent = GetAdjacentHex(hex, angle, hexDistance);
        //            var adjacent = currentHex
        //                .TranslatePolar(MathHelper.ToRadians(angle), hexDistance)
        //                .RoundTo(1);

        //            if (adjacent.X >= 0 && adjacent.Y >= 0 && adjacent.X <= area && adjacent.Y <= area)
        //            {
        //                if (!toExplore.Contains(adjacent) && !hexCenters.Contains(adjacent))
        //                    toExplore.Add(adjacent);
        //            }
        //        }

        //        hexCenters.Add(currentHex);
        //    }

        //    return hexCenters.Select(x => new Hex(x, hexSide));
        //}

        //private static Vector2 GetAdjacentHex(Vector2 hexCenter, float angleInDegree, float hexDistance)
        //{
        //    //var a1 = MathHelper.ToRadians(angleInDegree);
        //    //var c1 = (hexCenter + new Vector2((float)Math.Cos(a1) * hexDistance, (float)Math.Sin(a1) * hexDistance)).RoundTo(1);
        //    var c1 = hexCenter.TranslatePolar(MathHelper.ToRadians(angleInDegree), hexDistance).RoundTo(1);

        //    return c1;
        //}

        public bool Equals(Hex other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return other.Center.Equals(this.Center) && other.EdgeLength.Equals(this.EdgeLength) && other.Position.Equals(this.Position);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof(Hex)) return false;
            return Equals((Hex)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int result = this.Center.GetHashCode();
                result = (result * 397) ^ this.EdgeLength.GetHashCode();
                result = (result * 397) ^ this.Position.GetHashCode();
                return result;
            }
        }
    }
}
