using System;

namespace GameFramework
{
    public struct RectangleInt
    {
        private readonly int x;
        private readonly int y;
        private readonly int width;
        private readonly int height;

        public RectangleInt(int x, int y, int width, int height)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
        }

        public RectangleInt(int x, int y, Size size)
        {
            this.x = x;
            this.y = y;
            this.width = size.Width;
            this.height = size.Height;
        }

        public static RectangleInt Empty
        {
            get { return new RectangleInt(); }
        }

        public int X
        {
            get { return this.x; }
        }

        public int Y
        {
            get { return this.y; }
        }

        public int Width
        {
            get { return this.width; }
        }

        public int Height
        {
            get { return this.height; }
        }

        public int Left
        {
            get { return this.x; }
        }

        public int Right
        {
            get { return this.x + this.Width - 1; }
        }

        public int Top
        {
            get { return this.y; }
        }

        public int Bottom
        {
            get { return this.y + this.Height - 1; }
        }
        
        public Point Location
        {
            get { return new Point(this.x, this.y); }
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

        public bool Equals(RectangleInt other)
        {
            return other.x == this.x && other.y == this.y && other.width == this.width && other.height == this.height;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            if (obj.GetType() != typeof(RectangleInt))
            {
                return false;
            }
            return this.Equals((RectangleInt)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int result = this.x;
                result = (result * 397) ^ this.y;
                result = (result * 397) ^ this.width;
                result = (result * 397) ^ this.height;
                return result;
            }
        }

        public static bool operator ==(RectangleInt left, RectangleInt right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(RectangleInt left, RectangleInt right)
        {
            return !left.Equals(right);
        }

        public static RectangleInt FromBound(int left, int top, int right, int bottom)
        {
            return new RectangleInt(left, top, Math.Abs(right - left), Math.Abs(top - bottom));
        }
    }
}
