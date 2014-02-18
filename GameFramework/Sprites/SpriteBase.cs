using System;
using GameFramework.Cameras;

namespace GameFramework.Sprites
{
    public abstract class SpriteBase : INavigatorMetadataProvider
    {
        protected SpriteBase()
        {
            this.IsVisible = true;
            this.Color = Color.White;
        }

        public string SpriteName { get; set; }

        public Vector Position { get; set; }

        public float Rotation { get; set; }

        public Color Color { get; set; }

        public bool FlipHorizontally { get; set; }

        public bool FlipVertically { get; set; }

        public bool IsVisible { get; set; }

        public abstract int Draw(IDrawContext drawContext, Vector layerOffset, Vector parallaxScrollingVector, CameraMode cameraMode);

        public NavigatorMetadata GetMetadata()
        {
            return new NavigatorMetadata(this.SpriteName, NodeKind.Entity);
        }
    }
}