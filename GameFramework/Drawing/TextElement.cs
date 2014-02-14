using System;

using GameFramework.Cameras;
using GameFramework.Scenes;

namespace GameFramework.Drawing
{
    public class TextElement : DrawingElementBase
    {
        public TextElement(DrawingFont drawingFont, string text, Vector vector, Color color)
        {
            this.DrawingFont = drawingFont;
            this.Position = vector;
            this.Color = color;
            this.Text = text;
        }

        public DrawingFont DrawingFont { get; private set; }

        public Color Color { get; private set; }

        public string Text { get; private set; }

        public Vector Position { get; set; }

        public void SetParameters(params object[] parameters)
        {
            this.Parameters = parameters;
        }

        protected object[] Parameters { get; private set; }

        public override void Draw(IDrawContext drawContext, DrawingLayer drawingLayer)
        {
            var finalText = this.Parameters != null ? string.Format(this.Text, this.Parameters) : this.Text;
            var finalZoomFactor = drawingLayer.CameraMode == CameraMode.Fix ? 1.0f : drawContext.Camera.ZoomFactor;
            var finalVector = drawingLayer.CameraMode == CameraMode.Fix
                ? this.Position
                : this.Position
                    .Scale(drawContext.Camera.ZoomFactor)
                    .Translate(drawContext.Camera.GetSceneTranslationVector(drawingLayer.ParallaxScrollingVector));

            var param = new DrawStringParams
            {
                //DrawContext = drawContext,
                //Camera = camera,
                Text = finalText,
                Vector = finalVector,
                ZoomFactor = finalZoomFactor,
                DrawingFont = this.DrawingFont,
                Color = this.Color
            };

            drawContext.DrawString(param);
        }

        public override HitBase GetHit(Vector position, Camera camera, Vector layerOffset, Vector parallaxScrollingVector)
        {
            return null;
        }
    }
}