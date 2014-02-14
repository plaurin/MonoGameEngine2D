using System;

using GameFramework.Cameras;
using GameFramework.Scenes;

namespace GameFramework.Drawing
{
    // TODO: Could be renamed DrawingObject? LineObject, TextObject, etc.. to match Tiled object layer
    public abstract class DrawingElementBase
    {
        protected DrawingElementBase()
        {
            this.IsVisible = true;
        }

        public bool IsVisible { get; set; }

        public abstract void Draw(IDrawContext drawContext, DrawingLayer drawingLayer);

        public abstract HitBase GetHit(Vector position, Camera camera, Vector layerOffset, Vector parallaxScrollingVector);

        protected void DrawLine(IDrawContext drawContext, DrawingLayer drawingLayer, Vector fromVector, Vector toVector, int width, Color color)
        {
            var finalFrom = fromVector
                .Scale(drawContext.Camera.ZoomFactor)
                .Translate(drawContext.Camera.GetSceneTranslationVector(drawingLayer.ParallaxScrollingVector));

            var finalTo = toVector
                .Scale(drawContext.Camera.ZoomFactor)
                .Translate(drawContext.Camera.GetSceneTranslationVector(drawingLayer.ParallaxScrollingVector));

            var param = new DrawLineParams
            {
                VectorFrom = finalFrom,
                VectorTo = finalTo,
                Width = width * drawContext.Camera.ZoomFactor,
                Color = color
            };

            drawContext.DrawLine(param);
        }
    }
}