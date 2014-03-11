using System;
using GameFramework.Cameras;

namespace GameFramework.Layers
{
    public abstract class LayerBase : ILayer
    {
        protected LayerBase(string name)
        {
            this.Name = name;
            this.CameraMode = CameraMode.Follow;
            this.ParallaxScrollingVector = Vector.One;
            this.IsVisible = true;
            this.UseLinearSampler = null;
        }

        public string Name { get; set; }

        public CameraMode CameraMode { get; set; }

        public Vector ParallaxScrollingVector { get; set; }

        public Vector Offset { get; set; }

        public bool IsVisible { get; set; }

        public bool? UseLinearSampler { get; set; }

        public abstract int TotalElements { get; }

        public abstract int DrawnElementsLastFrame { get; }

        public abstract int Draw(IDrawContext drawContext, Transform transform);

        public NavigatorMetadata GetMetadata()
        {
            return new NavigatorMetadata(this.Name, NodeKind.Layer);
        }

        protected void SetSampler(IDrawContext drawContext)
        {
            if (this.UseLinearSampler.HasValue)
            {
                if (this.UseLinearSampler.Value)
                    drawContext.UseLinearSampler();
                else
                    drawContext.UsePointSampler();
            }
        }
    }
}