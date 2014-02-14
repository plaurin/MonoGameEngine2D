using System;

using GameFramework;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Color = Microsoft.Xna.Framework.Color;
using Viewport = Microsoft.Xna.Framework.Graphics.Viewport;

namespace MonoGameImplementation.EngineImplementation
{
    public class XnaDrawContext : IDrawImplementation
    {
        private readonly SpriteBatch spriteBatch;

        private readonly Texture2D blank;

        private readonly Viewport viewport;

        public XnaDrawContext(SpriteBatch spriteBatch, Texture2D blank, Viewport viewport)
        {
            this.spriteBatch = spriteBatch;
            this.blank = blank;
            this.viewport = viewport;
        }

        public void DrawString(DrawStringParams drawStringParams)
        {
            var font = (XnaDrawingFont)drawStringParams.DrawingFont;

            this.spriteBatch.DrawString(font.Font, drawStringParams.Text, drawStringParams.Vector.ToVector2(), drawStringParams.Color.ToXnaColor(), 
                0.0f, Vector2.Zero, drawStringParams.ZoomFactor, SpriteEffects.None, 0.0f);
        }

        public void DrawLine(DrawLineParams param)
        {
            var angle = (float)Math.Atan2(param.VectorTo.Y - param.VectorFrom.Y, param.VectorTo.X - param.VectorFrom.X);
            var length = Vector2.Distance(param.VectorFrom.ToVector2(), param.VectorTo.ToVector2());

            this.spriteBatch.Draw(this.blank, param.VectorFrom.ToVector2(), null, param.Color.ToXnaColor(), angle, Vector2.Zero,
                new Vector2(length, param.Width), SpriteEffects.None, 0);
        }

        public void DrawImage(DrawImageParams param)
        {
            var xnaTexture = (XnaTexture)param.Texture;

            var texture2D = xnaTexture.Texture2D;
            var destination = param.Destination.ToXnaRect();
            Microsoft.Xna.Framework.Rectangle? source = null;
            var rotation = param.Rotation;
            var origin = param.Origin.ToVector2();
            const SpriteEffects Effect = SpriteEffects.None;
            const int Depth = 0;

            if (param.Source.HasValue) source = param.Source.Value.ToXnaRect();

            this.spriteBatch.Draw(texture2D, destination, source, Color.White, rotation, origin, Effect, Depth);
        }

        public void FillColor(GameFramework.Color color)
        {
            var finalColor = new Color(color.R, color.G, color.B, 255) * (color.A / 255.0f);
            this.spriteBatch.Draw(this.blank, this.viewport.Bounds, finalColor);
        }
    }
}