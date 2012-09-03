using System;

namespace ClassLibrary
{
    public struct Point
    {
        private readonly int x;
        private readonly int y;

        public Point(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public static Point Zero
        {
            get { return new Point(); }
        }

        public int X
        {
            get { return this.x; }
        }

        public int Y
        {
            get { return this.y; }
        }

        public static Point operator -(Point point)
        {
            return new Point(-point.X, -point.Y);
        }

        public override string ToString()
        {
            return string.Format("{{X:{0} Y:{1}}}", this.X, this.Y);
        }
    }
}