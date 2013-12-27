using System;
using System.Collections.Generic;
using System.Linq;

namespace GameFramework.Hexes
{
    public class HexGridElement
    {
        public HexGridElement(Vector center, float edgeLength)
            : this(center, edgeLength, Point.Zero)
        {
        }

        public HexGridElement(Vector center, float edgeLength, Point position)
        {
            this.EdgeLength = edgeLength;
            this.Center = center;
            this.Position = position;

            this.Width = 2 * this.EdgeLength;
            this.Height = 2 * MathUtil.CalcHypotenuseSide(this.EdgeLength, this.EdgeLength / 2.0f);
            this.Radius = MathUtil.CalcHypotenuseSide(this.EdgeLength * 2, this.EdgeLength);
        }

        public Vector Center { get; private set; }

        public float EdgeLength { get; private set; }

        public Point Position { get; private set; }

        public Rectangle Rectangle
        {
            get
            {
                return new Rectangle(
                    (int)(this.Center.X - this.Width / 2.0), 
                    (int)(this.Center.Y - this.Height / 2.0),
                    (int)this.Width, 
                    (int)this.Height);
            }
        }

        public float Width { get; private set; }

        public float Height { get; private set; }

        public float Radius { get; private set; }

        public IEnumerable<Vector> GetVertices()
        {
            return Enumerable.Range(0, 6)
                .Select((x, i) => this.Center.TranslatePolar(MathUtil.ToRadians(60 + (i * 60)), this.EdgeLength));
        }

        public bool Equals(HexGridElement other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return other.Center.Equals(this.Center) && other.EdgeLength.Equals(this.EdgeLength) && other.Position.Equals(this.Position);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof(HexGridElement)) return false;
            return this.Equals((HexGridElement)obj);
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
