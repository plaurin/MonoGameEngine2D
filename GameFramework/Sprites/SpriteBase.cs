using System;
using GameFramework.Cameras;

namespace GameFramework.Sprites
{
    public abstract class SpriteBase : INavigatorMetadataProvider
    {
        protected SpriteBase()
        {
            this.IsVisible = true;
        }

        public string SpriteName { get; set; }

        public Vector Position { get; set; }

        public float Rotation { get; set; }

        public bool IsVisible { get; set; }

        public abstract int Draw(IDrawContext drawContext, Vector layerOffset, Vector parallaxScrollingVector, CameraMode cameraMode);

        public NavigatorMetadata GetMetadata()
        {
            return new NavigatorMetadata(this.SpriteName, NodeKind.Entity);
        }
    }
}