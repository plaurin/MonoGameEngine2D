using System;

namespace GameFramework
{
    /// <summary>
    /// <remarks>Should be used only for position in a map (array), Sprite (texture) position</remarks>
    /// </summary>
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

        public bool IsZero
        {
            get { return this.x == 0 && this.y == 0; }
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