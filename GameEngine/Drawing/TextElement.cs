using System;
using System.Xml.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using WindowsGame1.Cameras;

namespace WindowsGame1.Drawing
{
    public class TextElement : DrawingElementBase
    {
        private readonly DrawingFont drawingFont;
        private readonly string text;
        private readonly Vector2 vector;
        private readonly Color color;

        public TextElement(DrawingFont drawingFont, string text, Vector2 vector, Color color)
        {
            this.drawingFont = drawingFont;
            this.vector = vector;
            this.color = color;
            this.text = text;
        }

        public void SetParameters(params object[] parameters)
        {
            this.Parameters = parameters;
        }

        protected object[] Parameters { get; private set; }

        public override void Draw(SpriteBatch spriteBatch, Camera camera, DrawingMap drawingMap)
        {
            var finalText = this.Parameters != null ? string.Format(this.text, this.Parameters) : this.text;
            var finalZoomFactor = drawingMap.CameraMode == CameraMode.Fix ? 1.0f : camera.ZoomFactor;
            var finalVector = drawingMap.CameraMode == CameraMode.Fix
                ? this.vector
                : this.vector
                    .Scale(camera.ZoomFactor)
                    .Translate(camera.GetSceneTranslationVector(drawingMap.ParallaxScrollingVector));

            spriteBatch.DrawString(this.drawingFont.Font, finalText, finalVector, this.color, 0.0f, Vector2.Zero, 
                finalZoomFactor, SpriteEffects.None, 0.0f);
        }

        public override XElement ToXml()
        {
            return new XElement("TextElement",
                new XAttribute("fontName", this.drawingFont.Name),
                new XAttribute("text", this.text),
                new XAttribute("vector", this.vector),
                new XAttribute("color", this.color));
        }

        public static TextElement FromXml(GameResourceManager gameResourceManager, XElement element)
        {
            var fontName = element.Attribute("fontName").Value;
            var text = element.Attribute("text").Value;
            var vector = element.Attribute("vector").Value;
            var color = element.Attribute("color").Value;

            var font = gameResourceManager.GetDrawingFont(fontName);
            return new TextElement(font, text, MathUtil.ParseVector(vector), MathUtil.ParseColor(color));
        }
    }
}