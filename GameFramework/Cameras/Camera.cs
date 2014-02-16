using System;

namespace GameFramework.Cameras
{
    public class Camera : ICamera
    {
        private Vector innerPosition;

        public Camera(Viewport viewport)
        {
            this.Viewport = viewport;
            this.ZoomFactor = 1.0f;
            this.ViewPortCenter = new Vector(viewport.Width / 2.0f, viewport.Height / 2.0f);
            this.innerPosition = Vector.Zero;
            this.Position = Vector.Zero;
        }

        public Vector Position { get; private set; }

        public float ZoomFactor { get; set; }

        public Viewport Viewport { get; private set; }

        public Vector ViewPortCenter { get; private set; }

        public CameraCenter Center { get; set; }

        public Vector SceneTranslationVector
        {
            get
            {
                switch (this.Center)
                {
                    case CameraCenter.WindowCenter:
                        return new Vector(
                            this.ViewPortCenter.X - this.Position.X * this.ZoomFactor,
                            this.ViewPortCenter.Y - this.Position.Y * this.ZoomFactor);
                    case CameraCenter.WindowTopLeft:
                        return new Vector(
                            -this.Position.X * this.ZoomFactor,
                            -this.Position.Y * this.ZoomFactor);
                    default:
                        throw new NotSupportedException("CameraCenter value not supported");
                }
            }
        }

        public Viewport SceneViewport
        {
            get
            {
                switch (this.Center)
                {
                    case CameraCenter.WindowCenter:
                        return new Viewport(
                            this.Position.X - this.ViewPortCenter.X / this.ZoomFactor,
                            this.Position.Y - this.ViewPortCenter.Y / this.ZoomFactor,
                            this.Viewport.Width / this.ZoomFactor,
                            this.Viewport.Height / this.ZoomFactor);
                    case CameraCenter.WindowTopLeft:
                        return new Viewport(
                            this.Position.X / this.ZoomFactor,
                            this.Position.Y / this.ZoomFactor,
                            this.Viewport.Width / this.ZoomFactor,
                            this.Viewport.Height / this.ZoomFactor);
                    default:
                        throw new NotSupportedException("CameraCenter value not supported");
                }
            }
        }

        public void Move(float offsetX, float offsetY)
        {
            this.MoveTo(this.innerPosition.Translate(offsetX, offsetY));
        }

        public void MoveTo(float x, float y)
        {
            this.MoveTo(new Vector(x, y));
        }

        public void MoveTo(Vector newPosition)
        {
            this.innerPosition = newPosition;
            this.Position = this.innerPosition;
        }

        public Vector GetSceneTranslationVector(Vector parallaxScrollingVector)
        {
            switch (this.Center)
            {
                case CameraCenter.WindowCenter:
                    return new Vector(
                        this.ViewPortCenter.X - this.Position.X * this.ZoomFactor * parallaxScrollingVector.X,
                        this.ViewPortCenter.Y - this.Position.Y * this.ZoomFactor * parallaxScrollingVector.Y);
                case CameraCenter.WindowTopLeft:
                    return new Vector(
                        -this.Position.X * this.ZoomFactor * parallaxScrollingVector.X,
                        -this.Position.Y * this.ZoomFactor * parallaxScrollingVector.Y);
                default:
                    throw new NotSupportedException("CameraCenter value not supported");
            }
        }
    }
}
