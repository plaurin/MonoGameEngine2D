using System;
using System.Xml.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using WindowsGame1.Cameras;

namespace WindowsGame1.Drawing
{
    public class LineElement : DrawingElementBase
    {
        private readonly Texture2D blank;
        private readonly Vector2 fromVector;
        private readonly Vector2 toVector;
        private readonly int width;
        private readonly Color color;

        public LineElement(Texture2D blank, Vector2 fromVector, Vector2 toVector, int width, Color color)
        {
            this.blank = blank;
            this.fromVector = fromVector;
            this.toVector = toVector;
            this.width = width;
            this.color = color;
        }

        public override void Draw(SpriteBatch spriteBatch, Camera camera, DrawingMap drawingMap)
        {
            var finalFrom = this.fromVector
                .Scale(camera.ZoomFactor)
                .Translate(camera.GetSceneTranslationVector(drawingMap.ParallaxScrollingVector));

            var finalTo = this.toVector
                .Scale(camera.ZoomFactor)
                .Translate(camera.GetSceneTranslationVector(drawingMap.ParallaxScrollingVector));

            var finalWidth = this.width * camera.ZoomFactor;
            var angle = (float)Math.Atan2(finalTo.Y - finalFrom.Y, finalTo.X - finalFrom.X);
            var length = Vector2.Distance(finalFrom, finalTo);

            spriteBatch.Draw(this.blank, finalFrom, null, this.color, angle, Vector2.Zero,
                new Vector2(length, finalWidth), SpriteEffects.None, 0);
        }

        public override XElement ToXml()
        {
            return new XElement("LineElement",
                new XAttribute("from", this.fromVector),
                new XAttribute("to", this.toVector),
                new XAttribute("width", this.width),
                new XAttribute("color", this.color));
        }

        public static LineElement FromXml(GameResourceManager gameResourceManager, XElement element)
        {
            var from = element.Attribute("from").Value;
            var to = element.Attribute("to").Value;
            var width = element.Attribute("width").Value;
            var color = element.Attribute("color").Value;

            var blank = gameResourceManager.GetTexture("WhitePixel");
            return new LineElement(blank, MathUtil.ParseVector(from), MathUtil.ParseVector(to), int.Parse(width), MathUtil.ParseColor(color));
        }
    }
}