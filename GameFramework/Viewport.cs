using System;

namespace GameFramework
{
    public class Viewport
    {
        public Viewport(int width, int height)
        {
            this.Height = height;
            this.Width = width;
        }

        public int Width { get; private set; }

        public int Height { get; private set; }
    }
}