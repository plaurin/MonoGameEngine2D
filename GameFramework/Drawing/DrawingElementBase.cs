using System;

namespace GameFramework.Drawing
{
    // TODO: Could be renamed DrawingObject? LineObject, TextObject, etc.. to match Tiled object layer
    public abstract class DrawingElementBase : INavigatorMetadataProvider
    {
        protected DrawingElementBase()
        {
            this.IsVisible = true;
        }

        public bool IsVisible { get; set; }

        public abstract void Draw(IDrawContext drawContext, DrawingLayer drawingLayer);

        public NavigatorMetadata GetMetadata()
        {
            return new NavigatorMetadata(this.GetType().Name, NodeKind.Entity);
        }

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