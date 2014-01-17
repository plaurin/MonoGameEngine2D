using System;

namespace GameFramework
{
    public struct Rectangle
    {
        private readonly float x;
        private readonly float y;
        private readonly float width;
        private readonly float height;

        public Rectangle(float x, float y, float width, float height)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
        }

        public Rectangle(float x, float y, Size size)
        {
            this.x = x;
            this.y = y;
            this.width = size.Width;
            this.height = size.Height;
        }

        public static Rectangle Empty
        {
            get { return new Rectangle(); }
        }

        public float X
        {
            get { return this.x; }
        }

        public float Y
        {
            get { return this.y; }
        }

        public float Width
        {
            get { return this.width; }
        }

        public float Height
        {
            get { return this.height; }
        }

        public float Left
        {
            get { return this.x; }
        }

        public float Right
        {
            get { return this.x + this.Width - 1; }
        }

        public float Top
        {
            get { return this.y; }
        }

        public float Bottom
        {
            get { return this.y + this.Height - 1; }
        }
        
        public Vector Location
        {
            get { return new Vector(this.x, this.y); }
        }

        public bool Contains(Point point)
        {
            return point.X >= this.X && point.X <= this.X + this.Width
                   && point.Y >= this.Y && point.Y <= this.Y + this.Height;
        }

        public override string ToString()
        {
            return string.Format("{{X:{0} Y:{1} Width:{2} Height:{3}}}", this.X, this.Y, this.Width, this.Height);
        }

        public bool Equals(Rectangle other)
        {
            const float Tolerance = 0.0000001f;
            return Math.Abs(other.x - this.x) < Tolerance 
                && Math.Abs(other.y - this.y) < Tolerance 
                && Math.Abs(other.width - this.width) < Tolerance
                && Math.Abs(other.height - this.height) < Tolerance;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (obj.GetType() != typeof(Rectangle)) return false;
            return this.Equals((Rectangle)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = this.x.GetHashCode();
                hashCode = (hashCode * 397) ^ this.y.GetHashCode();
                hashCode = (hashCode * 397) ^ this.width.GetHashCode();
                hashCode = (hashCode * 397) ^ this.height.GetHashCode();
                return hashCode;
            }
        }

        public static bool operator ==(Rectangle left, Rectangle right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Rectangle left, Rectangle right)
        {
            return !left.Equals(right);
        }

        public static Rectangle FromBound(int left, int top, int right, int bottom)
        {
            return new Rectangle(left, top, Math.Abs(right - left), Math.Abs(top - bottom));
        }
    }
}