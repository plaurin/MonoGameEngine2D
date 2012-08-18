using System;

using ClassLibrary;
using ClassLibrary.Cameras;
using ClassLibrary.Maps;

namespace Editor
{
    public class WinColorMap : ColorMap
    {
        public WinColorMap(string name, Color color)
            : base(name, color)
        {
        }

        public override void Draw(DrawContext drawContext, Camera camera)
        {
            var winDrawContext = (WinDrawContext)drawContext;

            winDrawContext.FillColor(this.Color);
        }
    }
}