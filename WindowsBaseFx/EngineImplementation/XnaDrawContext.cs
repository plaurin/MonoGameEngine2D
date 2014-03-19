using System;
using System.Collections.Generic;
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
        private readonly IDictionary<IPreDrawable, RenderTargetWrap> renderTargetDictionary;

        private RenderTargetWrap renderTargetWrap;

        private bool isStarted;

        private bool isLinearSampler = true;

        public XnaDrawContext(SpriteBatch spriteBatch, Texture2D blank, Viewport viewport, 
            IDictionary<IPreDrawable, RenderTargetWrap> renderTargetDictionary)
        {
            this.spriteBatch = spriteBatch;
            this.blank = blank;
            this.renderTargetDictionary = renderTargetDictionary;
            this.viewport = viewport;
        }

        public void DrawString(DrawStringParams drawStringParams)
        {
            var font = (XnaDrawingFont)drawStringParams.DrawingFont;

            this.spriteBatch.DrawString(font.Font, drawStringParams.Text, drawStringParams.Vector.ToVector2(),
                drawStringParams.Color.ToXnaColor(), 0.0f, Vector2.Zero, drawStringParams.ZoomFactor, SpriteEffects.None, 0.0f);
        }

        public void DrawLine(DrawLineParams param)
        {
            var angle = (float)Math.Atan2(param.VectorTo.Y - param.VectorFrom.Y, param.VectorTo.X - param.VectorFrom.X);
            var length = Vector2.Distance(param.VectorFrom.ToVector2(), param.VectorTo.ToVector2());

            this.spriteBatch.Draw(this.blank, param.VectorFrom.ToVector2(), null, param.Color.ToXnaColor(), angle, Vector2.Zero,
                new Vector2(length, param.Width), SpriteEffects.None, 0);
        }

        public void DrawRectangle(DrawLineParams param)
        {
            var bottomLeft = new Vector(param.VectorFrom.X, param.VectorTo.Y);
            var topRight = new Vector(param.VectorTo.X, param.VectorFrom.Y);

            this.DrawLine(new DrawLineParams { VectorFrom = param.VectorFrom, VectorTo = topRight, Color = param.Color, Width = param.Width });
            this.DrawLine(new DrawLineParams { VectorFrom = topRight, VectorTo = param.VectorTo, Color = param.Color, Width = param.Width });
            this.DrawLine(new DrawLineParams { VectorFrom = param.VectorTo, VectorTo = bottomLeft, Color = param.Color, Width = param.Width });
            this.DrawLine(new DrawLineParams { VectorFrom = bottomLeft, VectorTo = param.VectorFrom, Color = param.Color, Width = param.Width });
        }

        public void FillRectangle(DrawLineParams param)
        {
            var rect = GameFramework.Rectangle.FromBound((int)param.VectorFrom.X, (int)param.VectorFrom.Y, (int)param.VectorTo.X, (int)param.VectorTo.Y);

            this.spriteBatch.Draw(this.blank, rect.ToXnaRect(), param.Color.ToXnaColor());
        }

        public void DrawImage(DrawImageParams param)
        {
            Microsoft.Xna.Framework.Rectangle? source = null;
            Microsoft.Xna.Framework.Vector2 scaleVector;

            var xnaTexture = (XnaTexture)param.Texture;

            var texture2D = xnaTexture.Texture2D;
            var destinationVector = param.Destination.Location.ToVector2();

            var color = new Color(param.Color.R, param.Color.G, param.Color.B, param.Color.A);
            var rotation = param.Rotation;
            var origin = param.Origin.ToVector2();
            var effect = (SpriteEffects)(int)param.ImageEffect;
            const int Depth = 0;

            if (param.Source.HasValue)
            {
                source = param.Source.Value.ToXnaRect();
                scaleVector = new Vector2(
                    param.Destination.Width / source.Value.Width,
                    param.Destination.Height / source.Value.Height);
            }
            else
            {
                scaleVector = new Vector2(
                    param.Destination.Width / texture2D.Width,
                    param.Destination.Height / texture2D.Height);
            }

            this.spriteBatch.Draw(texture2D, destinationVector, source, color, rotation, origin, scaleVector, effect, Depth);
        }

        public void FillColor(GameFramework.Color color)
        {
            var finalColor = new Color(color.R, color.G, color.B, 255) * (color.A / 255.0f);
            this.spriteBatch.Draw(this.blank, this.viewport.Bounds, finalColor);
        }

        public void UseLinearSampler()
        {
            if (!this.isLinearSampler || !this.isStarted)
            {
                if (this.isStarted) this.spriteBatch.End();

                this.isLinearSampler = true;
                this.SpriteBatchBegin();
            }
        }

        public void UsePointSampler()
        {
            if (this.isLinearSampler || !this.isStarted)
            {
                if (this.isStarted) this.spriteBatch.End();

                this.isLinearSampler = false;
                this.SpriteBatchBegin();
            }
        }

        public void SetRenderTarget(IPreDrawable painter, Vector size)
        {
            if (!this.renderTargetDictionary.TryGetValue(painter, out this.renderTargetWrap))
            {
                this.renderTargetWrap = new RenderTargetWrap();
                this.renderTargetDictionary.Add(painter, this.renderTargetWrap);
            }

            if (size.X > this.renderTargetWrap.ActualSize.X || size.Y > this.renderTargetWrap.ActualSize.Y)
            {
                this.renderTargetWrap.ActualSize = new Vector((int)(size.X * 1.2f), (int)(size.Y * 1.2f));

                this.renderTargetWrap.RenderTarget2D = new RenderTarget2D(this.spriteBatch.GraphicsDevice,
                    (int)this.renderTargetWrap.ActualSize.X, (int)this.renderTargetWrap.ActualSize.Y);
            }

            this.renderTargetWrap.Size = size;

            this.spriteBatch.GraphicsDevice.SetRenderTarget(this.renderTargetWrap.RenderTarget2D);

            this.SpriteBatchBegin();
            this.spriteBatch.GraphicsDevice.Clear(GameFramework.Color.Transparent.ToXnaColor());
        }

        public void FlushRenderTarget()
        {
            this.spriteBatch.End();

            this.spriteBatch.GraphicsDevice.SetRenderTarget(null);
        }

        public void DrawPreDrawn(IPreDrawable painter)
        {
            if (this.renderTargetDictionary.TryGetValue(painter, out this.renderTargetWrap))
            {
                this.spriteBatch.Draw(this.renderTargetWrap.RenderTarget2D, this.spriteBatch.GraphicsDevice.Viewport.Bounds,
                    new Microsoft.Xna.Framework.Rectangle(0, 0, (int)this.renderTargetWrap.Size.X, (int)this.renderTargetWrap.Size.Y),
                    Color.White);
            }
        }

        private void SpriteBatchBegin()
        {
            if (this.isLinearSampler)
                this.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);
            else
                this.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied,
                    SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise);

            this.isStarted = true;
        }
    }

    public class RenderTargetWrap
    {
        public RenderTarget2D RenderTarget2D { get; set; }

        public Vector Size { get; set; }

        public Vector ActualSize { get; set; }
    }
}