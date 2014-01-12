using System;
using System.Xml.Linq;

using GameFramework.Cameras;
using GameFramework.Scenes;

namespace GameFramework.Drawing
{
    public class TextElement : DrawingElementBase
    {
        private readonly DrawingFont drawingFont;

        private readonly Color color;

        public TextElement(DrawingFont drawingFont, string text, Vector vector, Color color)
        {
            this.drawingFont = drawingFont;
            this.Position = vector;
            this.color = color;
            this.Text = text;
        }

        public string Text { get; private set; }

        public Vector Position { get; set; }

        public void SetParameters(params object[] parameters)
        {
            this.Parameters = parameters;
        }

        protected object[] Parameters { get; private set; }

        public override void Draw(DrawContext drawContext, Camera camera, DrawingMap drawingMap)
        {
            var finalText = this.Parameters != null ? string.Format(this.Text, this.Parameters) : this.Text;
            var finalZoomFactor = drawingMap.CameraMode == CameraMode.Fix ? 1.0f : camera.ZoomFactor;
            var finalVector = drawingMap.CameraMode == CameraMode.Fix
                ? this.Position
                : this.Position
                    .Scale(camera.ZoomFactor)
                    .Translate(camera.GetSceneTranslationVector(drawingMap.ParallaxScrollingVector));

            drawContext.DrawString(drawContext, camera, finalText, finalVector, finalZoomFactor, this.drawingFont, this.color);
            //spriteBatch.DrawString(this.drawingFont.Font, finalText, finalVector, this.color, 0.0f, Vector2.Zero, 
            //    finalZoomFactor, SpriteEffects.None, 0.0f);
        }

        public override XElement ToXml()
        {
            return new XElement("TextElement",
                new XAttribute("fontName", this.drawingFont.Name),
                new XAttribute("text", this.Text),
                new XAttribute("vector", this.Position),
                new XAttribute("color", this.color));
        }

        public override HitBase GetHit(Point position, Camera camera, Point mapOffset, Vector parallaxScrollingVector)
        {
            return null;
        }

        public static TextElement FromXml(GameResourceManager gameResourceManager, XElement element)
        {
            var fontName = element.Attribute("fontName").Value;
            var text = element.Attribute("text").Value;
            var vector = element.Attribute("vector").Value;
            var color = element.Attribute("color").Value;

            //var font = gameResourceManager.GetDrawingFont(fontName); OnlyXNA
            var font = new DrawingFont { Name = fontName };
            return new TextElement(font, text, MathUtil.ParseVector(vector), MathUtil.ParseColor(color));
        }
    }
}