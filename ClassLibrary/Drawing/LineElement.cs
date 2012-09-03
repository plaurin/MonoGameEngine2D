using System;
using System.Xml.Linq;

using ClassLibrary.Cameras;

namespace ClassLibrary.Drawing
{
    public class LineElement : DrawingElementBase
    {
        private readonly Texture blank;
        private readonly Vector fromVector;
        private readonly Vector toVector;
        private readonly int width;
        private readonly Color color;

        public LineElement(Texture blank, Vector fromVector, Vector toVector, int width, Color color)
        {
            this.blank = blank;
            this.fromVector = fromVector;
            this.toVector = toVector;
            this.width = width;
            this.color = color;
        }

        public override void Draw(DrawContext drawContext, Camera camera, DrawingMap drawingMap)
        {
            var finalFrom = drawingMap.CameraMode == CameraMode.Fix ? this.fromVector : this.fromVector
                .Scale(camera.ZoomFactor)
                .Translate(camera.GetSceneTranslationVector(drawingMap.ParallaxScrollingVector));

            var finalTo = drawingMap.CameraMode == CameraMode.Fix ? this.toVector : this.toVector
                .Scale(camera.ZoomFactor)
                .Translate(camera.GetSceneTranslationVector(drawingMap.ParallaxScrollingVector));

            var zoom = drawingMap.CameraMode == CameraMode.Fix ? 1.0f : this.width * camera.ZoomFactor;

            //var finalWidth = this.width * camera.ZoomFactor;
            //var angle = (float)Math.Atan2(finalTo.Y - finalFrom.Y, finalTo.X - finalFrom.X);
            //var length = Vector.Distance(finalFrom, finalTo);

            drawContext.DrawLine(finalFrom, finalTo, zoom, this.color);
            //spriteBatch.Draw(this.blank, finalFrom, null, this.color, angle, Vector.Zero,
            //    new Vector(length, finalWidth), SpriteEffects.None, 0);
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