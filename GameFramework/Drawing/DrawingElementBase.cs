using System;
using System.Xml.Linq;

using GameFramework.Cameras;

namespace GameFramework.Drawing
{
    // TODO: Could be renamed DrawingObject? LineObject, TextObject, etc.. to match Tiled object layer
    public abstract class DrawingElementBase
    {
        public abstract void Draw(DrawContext drawContext, Camera camera, DrawingMap drawingMap);

        public abstract XElement ToXml();

        protected void DrawLine(DrawContext drawContext, Camera camera, DrawingMap drawingMap, Vector fromVector, Vector toVector, int width, Color color)
        {
            var finalFrom = fromVector
                .Scale(camera.ZoomFactor)
                .Translate(camera.GetSceneTranslationVector(drawingMap.ParallaxScrollingVector));

            var finalTo = toVector
                .Scale(camera.ZoomFactor)
                .Translate(camera.GetSceneTranslationVector(drawingMap.ParallaxScrollingVector));

            drawContext.DrawLine(finalFrom, finalTo, width * camera.ZoomFactor, color);
        }
    }
}