using System;

using GameFramework.Cameras;
using GameFramework.Scenes;

namespace GameFramework.Drawing
{
    // TODO: Could be renamed DrawingObject? LineObject, TextObject, etc.. to match Tiled object layer
    public abstract class DrawingElementBase
    {
        public abstract void Draw(DrawContext drawContext, Camera camera, DrawingLayer drawingLayer);

        public abstract HitBase GetHit(Point position, Camera camera, Point layerOffset, Vector parallaxScrollingVector);

        protected void DrawLine(DrawContext drawContext, Camera camera, DrawingLayer drawingLayer, Vector fromVector, Vector toVector, int width, Color color)
        {
            var finalFrom = fromVector
                .Scale(camera.ZoomFactor)
                .Translate(camera.GetSceneTranslationVector(drawingLayer.ParallaxScrollingVector));

            var finalTo = toVector
                .Scale(camera.ZoomFactor)
                .Translate(camera.GetSceneTranslationVector(drawingLayer.ParallaxScrollingVector));

            drawContext.DrawLine(finalFrom, finalTo, width * camera.ZoomFactor, color);
        }
    }
}