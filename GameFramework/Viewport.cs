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

        public override string ToString()
        {
            return string.Format("{{X:{0} Y:{1} Width:{2} Height:{3}}}", this.X, this.Y, this.Width, this.Height);
        }

        public bool IsVisible(Rectangle destination)
        {
            return destination.Left <= this.X + this.Width
                && destination.Right >= this.X
                && destination.Top <= this.Y + this.Height
                && destination.Bottom >= this.Y;
        }
    }
}