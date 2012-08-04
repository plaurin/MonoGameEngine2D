using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WindowsGame1.Maps
{
    public class ImageMap : MapBase
    {
        private readonly Texture2D texture;

        public ImageMap(Texture2D texture, Rectangle rectangle)
        {
            this.texture = texture;
            this.Rectangle = rectangle;
        }

        public Rectangle Rectangle { get; set; }

        public static ImageMap CreateFillScreenImageMap(GraphicsDevice device, Texture2D texture)
        {
            return new ImageMap(texture, device.Viewport.Bounds);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.texture, this.Rectangle, Color.White);
        }
    }
}