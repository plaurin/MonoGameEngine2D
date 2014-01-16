using System;
using GameFramework.Cameras;
using GameFramework.Drawing;
using GameFramework.Inputs;
using GameFramework.Layers;

namespace GameFramework.Utilities
{
    public class MouseCursorLayer : LayerBase
    {
        private readonly LayerBase layer;
        private readonly Action<MouseStateBase> updateAction;

        private MouseCursorLayer(LayerBase layer, Action<MouseStateBase> updateAction)
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
                drawingMap.AddLine(mouseState.AbsolutePosition.Translate(-10, 0).ToVector(),
                    mouseState.AbsolutePosition.Translate(10, 0).ToVector(), 2, Color.Red);

                drawingMap.AddLine(mouseState.AbsolutePosition.Translate(0, -10).ToVector(),
                    mouseState.AbsolutePosition.Translate(0, 10).ToVector(), 2, Color.Red);
            };

            return new MouseCursorLayer(drawingMap, updateAction);
        }

        public void Update(MouseStateBase mouseState)
        {
            this.updateAction(mouseState);
        }

        public override void Draw(DrawContext drawContext, Camera camera)
        {
            this.layer.Draw(drawContext, camera);
        }
    }
}