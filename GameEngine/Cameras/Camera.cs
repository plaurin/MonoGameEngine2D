using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WindowsGame1.Cameras
{
    public class Camera
    {
        private readonly Viewport viewport;

        public Camera(Viewport viewport)
        {
            this.viewport = viewport;
            this.ZoomFactor = 1.0f;
            this.ViewPortCenter = new Point(viewport.Width / 2, viewport.Height / 2);
        }

        public Point Position { get; private set; }

        public float ZoomFactor { get; set; }

        public Point ViewPortCenter { get; private set; }

        public Point SceneTranslationVector
        {
            get
            {
                return new Point(
                    (int)(this.ViewPortCenter.X - this.Position.X * this.ZoomFactor),
                    (int)(this.ViewPortCenter.Y - this.Position.Y * this.ZoomFactor));
            }
        }

        public Rectangle SceneViewPort
        {
            get
            {
                return new Rectangle(
                    (int)(this.Position.X - this.ViewPortCenter.X / this.ZoomFactor),
                    (int)(this.Position.Y - this.ViewPortCenter.Y / this.ZoomFactor),
                    (int)(this.viewport.Width / this.ZoomFactor),
                    (int)(this.viewport.Height / this.ZoomFactor));
            }
        }

        public void Move(int delatX, int deltaY)
        {
            this.Position = this.Position.Translate(new Point(delatX, deltaY));
        }

        public Point GetSceneTranslationVector(Vector2 parallaxScrollingVector)
        {
            return new Point(
                (int)(this.ViewPortCenter.X - this.Position.X * this.ZoomFactor * parallaxScrollingVector.X),
                (int)(this.ViewPortCenter.Y - this.Position.Y * this.ZoomFactor * parallaxScrollingVector.Y));
        }
    }
}
