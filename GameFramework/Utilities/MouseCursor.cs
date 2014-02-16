using System;
using GameFramework.Inputs;

namespace GameFramework.Utilities
{
    public class MouseCursor : IDrawable, INavigatorMetadataProvider
    {
        private Vector mousePosition;

        public MouseCursor(MouseTracking mouseTracking)
        {
            mouseTracking.OnMove((mt, gt) => this.mousePosition = mt.AbsolutePosition);
        }

        public int Draw(IDrawContext drawContext)
        {
            DrawLine(drawContext, this.mousePosition.Translate(-11, 0), this.mousePosition.Translate(10, 0));
            DrawLine(drawContext, this.mousePosition.Translate(0, -11), this.mousePosition.Translate(0, 10));
            return 2;
        }

        public NavigatorMetadata GetMetadata()
        {
            return new NavigatorMetadata("Mouse Cusor", NodeKind.Utility);
        }

        private static void DrawLine(IDrawContext drawContext, Vector from, Vector to)
        {
            drawContext.DrawLine(new DrawLineParams { VectorFrom = from, VectorTo = to, Width = 2, Color = Color.Red });
        }
    }
}