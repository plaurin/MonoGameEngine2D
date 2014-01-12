using System;
using System.Xml.Linq;
using GameFramework.Cameras;
using GameFramework.Drawing;
using GameFramework.Inputs;
using GameFramework.Maps;

namespace GameFramework.Utilities
{
    public class MouseCursorMap : MapBase
    {
        private readonly MapBase map;
        private readonly Action<MouseStateBase> updateAction;

        private MouseCursorMap(MapBase map, Action<MouseStateBase> updateAction)
            : base("MouseCursor")
        {
            this.map = map;
            this.updateAction = updateAction;
        }

        public static MouseCursorMap Create(GameResourceManager gameResourceManager)
        {
            var drawingMap = new DrawingMap("MouseCursorInner", gameResourceManager) { CameraMode = CameraMode.Fix };
            Action<MouseStateBase> updateAction = mouseState =>
            {
                drawingMap.ClearAll();
                drawingMap.AddLine(mouseState.AbsolutePosition.Translate(-10, 0).ToVector(),
                    mouseState.AbsolutePosition.Translate(10, 0).ToVector(), 2, Color.Red);

                drawingMap.AddLine(mouseState.AbsolutePosition.Translate(0, -10).ToVector(),
                    mouseState.AbsolutePosition.Translate(0, 10).ToVector(), 2, Color.Red);
            };

            return new MouseCursorMap(drawingMap, updateAction);
        }

        public void Update(MouseStateBase mouseState)
        {
            this.updateAction(mouseState);
        }

        public override void Draw(DrawContext drawContext, Camera camera)
        {
            this.map.Draw(drawContext, camera);
        }

        public override XElement ToXml()
        {
            throw new NotImplementedException();
        }
    }
}