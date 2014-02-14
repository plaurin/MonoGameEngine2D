using System;
using GameFramework.Cameras;
using GameFramework.Drawing;
using GameFramework.Inputs;
using GameFramework.Layers;

namespace GameFramework.Utilities
{
    public class MouseCursorLayer : LayerBase
    {
        private readonly ILayer layer;
        private readonly Action<MouseStateBase> updateAction;

        private MouseCursorLayer(ILayer layer, Action<MouseStateBase> updateAction)
            : base("MouseCursor")
        {
            this.layer = layer;
            this.updateAction = updateAction;
        }

        public static MouseCursorLayer Create(GameResourceManager gameResourceManager)
        {
            var drawingMap = new DrawingLayer("MouseCursorInner", gameResourceManager) { CameraMode = CameraMode.Fix };
            Action<MouseStateBase> updateAction = mouseState =>
            {
                drawingMap.ClearAll();
                drawingMap.AddLine(mouseState.AbsolutePosition.Translate(-10, 0),
                    mouseState.AbsolutePosition.Translate(10, 0), 2, Color.Red);

                drawingMap.AddLine(mouseState.AbsolutePosition.Translate(0, -10),
                    mouseState.AbsolutePosition.Translate(0, 10), 2, Color.Red);
            };

            return new MouseCursorLayer(drawingMap, updateAction);
        }

        public void Update(MouseStateBase mouseState)
        {
            this.updateAction(mouseState);
        }

        public override int TotalElements
        {
            get { return this.layer.TotalElements; }
        }

        public override int DrawnElementsLastFrame
        {
            get { return this.layer.DrawnElementsLastFrame; }
        }

        public override int Draw(DrawContext drawContext)
        {
            return this.layer.Draw(drawContext);
        }
    }
}