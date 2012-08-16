using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using WindowsGame1.Cameras;

namespace WindowsGame1.Drawing
{
    public class PolygonElement : DrawingElementBase
    {
        private readonly Texture2D blank;
        private readonly List<Vector2> vertices;
        private readonly int width;
        private readonly Color color;

        public PolygonElement(Texture2D blank, IEnumerable<Vector2> vertices, int width, Color color)
        {
            this.blank = blank;
            this.vertices = new List<Vector2>(vertices);
            this.width = width;
            this.color = color;
        }

        public override void Draw(SpriteBatch spriteBatch, Camera camera, DrawingMap drawingMap)
        {
            this.vertices.ForEachPair((x, y) => this.DrawLine(spriteBatch, camera, drawingMap, x, y));
        }

        private void DrawLine(SpriteBatch spriteBatch, Camera camera, DrawingMap drawingMap, Vector2 fromVector, Vector2 toVector)
        {
            var finalFrom = fromVector
                .Scale(camera.ZoomFactor)
                .Translate(camera.GetSceneTranslationVector(drawingMap.ParallaxScrollingVector));

            var finalTo = toVector
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
            return new XElement("PolygonElement",
                new XAttribute("width", this.width),
                new XAttribute("color", this.color),
                new XElement("Vertices", string.Join(", ", this.vertices.ToArray())));
        }

        public static PolygonElement FromXml(GameResourceManager gameResourceManager, XElement element)
        {
            var width = element.Attribute("width").Value;
            var color = element.Attribute("color").Value;
            var vertices = element.Element("Vertices").Value.Split(',').Select(x => MathUtil.ParseVector(x.Trim()));

            var blank = gameResourceManager.GetTexture("WhitePixel");
            return new PolygonElement(blank, vertices, int.Parse(width), MathUtil.ParseColor(color));
        }
    }
}