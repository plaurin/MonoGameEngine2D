using System;

namespace GameFramework.Sprites
{
    public abstract class SpriteBase : IDrawable, INavigatorMetadataProvider
    {
        protected SpriteBase()
        {
            this.IsVisible = true;
            this.Scale = 1.0f;
            this.Color = Color.White;
        }

        public string SpriteName { get; set; }

        public Vector Position { get; set; }

        public float Rotation { get; set; }

        public float Scale { get; set; }

        public Color Color { get; set; }

        public bool IsVisible { get; set; }

        public abstract int Draw(IDrawContext drawContext, Transform transform);

        public NavigatorMetadata GetMetadata()
        {
            return new NavigatorMetadata(this.SpriteName, NodeKind.Entity);
        }
    }
}