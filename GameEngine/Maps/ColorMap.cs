using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using WindowsGame1.Cameras;

namespace WindowsGame1.Maps
{
    public class ColorMap : MapBase
    {
        private readonly Color color;
        private readonly Texture2D texture;

        public ColorMap(GraphicsDevice device, Color color)
        {
            this.color = color;
            this.texture = CreateTexture(device);
        }

        public void Draw(SpriteBatch spriteBatch, Camera camera)
        {
            spriteBatch.Draw(this.texture, camera.Viewport.Bounds, this.color);
        }

        private static Texture2D CreateTexture(GraphicsDevice device)
        {
            var rectangleTexture = new Texture2D(device, 1, 1, false, SurfaceFormat.Color);
            rectangleTexture.SetData(new[] { Color.White });
            return rectangleTexture;
        }
    }
}