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

        public override void DrawImage(Texture texture, Rectangle source, Rectangle destination)
        {
            var xnaTexture = (XnaTexture)texture;
            this.spriteBatch.Draw(xnaTexture.Texture2D, destination.ToXnaRect(), source.ToXnaRect(), Color.White);
        }

        public override void DrawImage(Texture texture, Rectangle destination)
        {
            var xnaTexture = (XnaTexture)texture;
            this.spriteBatch.Draw(xnaTexture.Texture2D, destination.ToXnaRect(), Color.White);
        }

        public override void FillColor(GameFramework.Color color)
        {
            var finalColor = new Color(color.R, color.G, color.B, 255) * (color.A / 255.0f);
            this.spriteBatch.Draw(this.blank, this.viewport.Bounds, finalColor);
        }
    }
}