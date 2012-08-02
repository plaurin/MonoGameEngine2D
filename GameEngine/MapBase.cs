using System;

using Microsoft.Xna.Framework;

namespace WindowsGame1
{
    public class MapBase
    {
        public MapBase()
        {
            this.Scaling = 1.0f;
            this.Zooming = 1.0f;
            this.Position = Point.Zero;
        }

        public float Scaling { get; set; }

        public float Zooming { get; set; }

        public Point Position { get; set; }
    }
}
