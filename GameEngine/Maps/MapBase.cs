using System;
using System.Collections.Generic;
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

        public abstract XElement ToXml();

        public IEnumerable<object> BaseToXml()
        {
            yield return new XAttribute("parallaxScrollingVector", this.ParallaxScrollingVector);
        }

        public void BaseFromXml(XElement element)
        {
            this.ParallaxScrollingVector = MathUtil.ParseVector(element.Attribute("parallaxScrollingVector").Value);
        }
    }
}
