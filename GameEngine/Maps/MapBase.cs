using System;

using Microsoft.Xna.Framework;

namespace WindowsGame1.Maps
{
    public class MapBase
    {
        public MapBase()
        {
            this.ParallaxScrollingVector = Vector2.One;
        }

        public Vector2 ParallaxScrollingVector { get; set; }
    }
}
