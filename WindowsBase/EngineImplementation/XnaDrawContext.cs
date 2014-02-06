using System;

using GameFramework;
using GameFramework.Cameras;
using GameFramework.Drawing;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Color = Microsoft.Xna.Framework.Color;
using Rectangle = GameFramework.Rectangle;
using Texture = GameFramework.Texture;
using Viewport = Microsoft.Xna.Framework.Graphics.Viewport;

namespace MonoGameImplementation.EngineImplementation
{
    public class XnaDrawContext : DrawContext
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

        public override void DrawString(DrawContext drawContext, Camera camera, string text, Vector vector, float zoomFactor, DrawingFont drawingFont, GameFramework.Color color)
        {
            var font = (XnaDrawingFont)drawingFont;

            this.spriteBatch.DrawString(font.Font, text, vector.ToVector2(), color.ToXnaColor(), 
                0.0f, Vector2.Zero, zoomFactor, SpriteEffects.None, 0.0f);
        }

        public override void DrawLine(Vector vectorFrom, Vector vectorTo, float width, GameFramework.Color color)
        {
            var angle = (float)Math.Atan2(vectorTo.Y - vectorFrom.Y, vectorTo.X - vectorFrom.X);
            var length = Vector2.Distance(vectorFrom.ToVector2(), vectorTo.ToVector2());

            this.spriteBatch.Draw(this.blank, vectorFrom.ToVector2(), null, color.ToXnaColor(), angle, Vector2.Zero, 
                new Vector2(length, width), SpriteEffects.None, 0);
        }

        public override void DrawImage(DrawImageParams param)
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

        public override void FillColor(GameFramework.Color color)
        {
            var finalColor = new Color(color.R, color.G, color.B, 255) * (color.A / 255.0f);
            this.spriteBatch.Draw(this.blank, this.viewport.Bounds, finalColor);
        }
    }
}