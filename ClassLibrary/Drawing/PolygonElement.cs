using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

using ClassLibrary.Cameras;

namespace ClassLibrary.Drawing
{
    public class PolygonElement : DrawingElementBase
    {
        private readonly Texture blank;
        private readonly List<Vector> vertices;
        private readonly int width;
        private readonly Color color;

        public PolygonElement(Texture blank, IEnumerable<Vector> vertices, int width, Color color)
        {
            this.blank = blank;
            this.vertices = new List<Vector>(vertices);
            this.width = width;
            this.color = color;
        }

        public override void Draw(DrawContext drawContext, Camera camera, DrawingMap drawingMap)
        {
            this.vertices.ForEachPair((x, y) => this.DrawLine(drawContext, camera, drawingMap, x, y));
        }

        public override XElement ToXml()
        {
            return new XElement("PolygonElement",
                new XAttribute("width", this.width),
                new XAttribute("color", this.color),
                new XElement("Vertices", string.Join(", ", this.vertices)));
        }

        public static PolygonElement FromXml(Factory resourceManager, GameResourceManager gameResourceManager, XElement element)
        {
            var width = element.Attribute("width").Value;
            var color = element.Attribute("color").Value;
            var vertices = element.Element("Vertices").Value.Split(',').Select(x => MathUtil.ParseVector(x.Trim()));

            //var blank = gameResourceManager.GetTexture("WhitePixel"); OnlyXNA
            Texture blank = null;
            return new PolygonElement(blank, vertices, int.Parse(width), MathUtil.ParseColor(color));
        }

        private void DrawLine(DrawContext drawContext, Camera camera, DrawingMap drawingMap, Vector fromVector, Vector toVector)
        {
            var finalFrom = fromVector
                .Scale(camera.ZoomFactor)
                .Translate(camera.GetSceneTranslationVector(drawingMap.ParallaxScrollingVector));

            var finalTo = toVector
                .Scale(camera.ZoomFactor)
                .Translate(camera.GetSceneTranslationVector(drawingMap.ParallaxScrollingVector));

            //var finalWidth = this.width * camera.ZoomFactor;
            //var angle = (float)Math.Atan2(finalTo.Y - finalFrom.Y, finalTo.X - finalFrom.X);
            //var length = Vector.Distance(finalFrom, finalTo);

            drawContext.DrawLine(finalFrom, finalTo, this.width * camera.ZoomFactor, this.color);
            //spriteBatch.Draw(this.blank, finalFrom, null, this.color, angle, Vector2.Zero,
            //    new Vector2(length, finalWidth), SpriteEffects.None, 0);
        }
    }
}