using System;

namespace ClassLibrary
{
    public struct Rectangle
    {
        private readonly int x;
        private readonly int y;
        private readonly int width;
        private readonly int height;

        public Rectangle(int x, int y, int width, int height)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
        }

        public static Rectangle Empty
        {
            get { return new Rectangle(); }
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

        public Point Location
        {
            get { return new Point(this.x, this.y); }
        }

        public bool Contains(Point point)
        {
            return point.X >= this.X && point.X <= this.X + this.Width
                && point.Y >= this.Y && point.Y <= this.Y + this.Height;
        }
    }
}
