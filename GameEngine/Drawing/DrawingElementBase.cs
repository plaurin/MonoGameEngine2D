using System;
using System.Xml.Linq;

using Microsoft.Xna.Framework.Graphics;

using WindowsGame1.Cameras;

namespace WindowsGame1.Drawing
{
    public abstract class DrawingElementBase
    {
        public abstract void Draw(SpriteBatch spriteBatch, Camera camera, DrawingMap drawingMap);

        public abstract XElement ToXml();
    }
}