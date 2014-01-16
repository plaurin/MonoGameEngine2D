using System;

namespace GameFramework
{
    public class Viewport
    {
        public Viewport(float x, float y, float width, float height)
            : this(width, height)
        {
            this.X = x;
            this.Y = y;
            this.Positon = new Vector(x, y);
        }

        public Viewport(float width, float height)
        {
            this.Width = width;
            this.Height = height;
            this.Size = new Vector(width, height);
        }

        public float X { get; private set; }

        public float Y { get; private set; }

        public float Width { get; private set; }

        public float Height { get; private set; }

        public Vector Positon { get; private set; }

        public Vector Size { get; private set; }
    }
}