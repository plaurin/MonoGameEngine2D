using System;

using ClassLibrary;
using ClassLibrary.Cameras;
using ClassLibrary.Sprites;

namespace Editor
{
    internal class WinSpriteSheet : SpriteSheet
    {
        public WinSpriteSheet(Texture texture, string name)
            : base(texture, name)
        {
        }

        protected override void DoDraw(DrawContext drawContext, Camera camera, Rectangle source, Rectangle destination)
        {
            var winDrawContext = (WinDrawContext)drawContext;

            winDrawContext.DrawImage(this.Texture, source, destination);
        }
    }
}