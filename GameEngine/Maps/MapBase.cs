using System;
using System.Xml.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using WindowsGame1.Cameras;

namespace WindowsGame1.Maps
{
    public abstract class MapBase
    {
        protected MapBase()
        {
            this.ParallaxScrollingVector = Vector2.One;
        }

        public Vector2 ParallaxScrollingVector { get; set; }

        public abstract void Draw(SpriteBatch spriteBatch, Camera camera);

        public abstract XElement GetXml();
    }
}
