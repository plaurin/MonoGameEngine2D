﻿using System;

namespace ClassLibrary.Cameras
{
    public class Camera
    {
        private Vector innerPosition;

        public Camera(Viewport viewport)
        {
            this.Viewport = viewport;
            this.ZoomFactor = 1.0f;
            this.ViewPortCenter = new Point(viewport.Width / 2, viewport.Height / 2);
            this.innerPosition = Vector.Zero;
            this.Position = Point.Zero;
        }

        public Point Position { get; private set; }

        public float ZoomFactor { get; set; }

        public Viewport Viewport { get; private set; }

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
                    (int)(this.Viewport.Width / this.ZoomFactor),
                    (int)(this.Viewport.Height / this.ZoomFactor));
            }
        }

        public void Move(float offsetX, float offsetY)
        {
            this.innerPosition = this.innerPosition.Translate(offsetX, offsetY);
            this.Position = this.innerPosition.ToPoint();
        }

        public Point GetSceneTranslationVector(Vector parallaxScrollingVector)
        {
            return new Point(
                (int)(this.ViewPortCenter.X - this.Position.X * this.ZoomFactor * parallaxScrollingVector.X),
                (int)(this.ViewPortCenter.Y - this.Position.Y * this.ZoomFactor * parallaxScrollingVector.Y));
        }
    }

    public enum CameraMode
    {
        Follow = 0,
        Fix = 1
    }
}