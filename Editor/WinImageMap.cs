using System;

using ClassLibrary;
using ClassLibrary.Cameras;
using ClassLibrary.Maps;

namespace Editor
{
    public class WinImageMap : ImageMap
    {
        public WinImageMap(string name, Texture texture, Rectangle rectangle)
            : base(name, texture, rectangle)
        {
        }

        public override void Draw(DrawContext drawContext, Camera camera)
        {
            var winDrawContext = (WinDrawContext)drawContext;

            winDrawContext.DrawImage(this.Texture, this.Rectangle);
        }
    }
}