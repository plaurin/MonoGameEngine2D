using GameFramework.Cameras;
using GameFramework.Scenes;

namespace GameFramework.Sprites
{
    public abstract class SpriteBase
    {
        public string SpriteName { get; set; }

        public Vector Position { get; set; }

        public abstract void Draw(DrawContext drawContext, Camera camera, Vector layerOffset, Vector parallaxScrollingVector);

        public abstract HitBase GetHit(Vector position, Camera camera, Vector layerOffset, Vector parallaxScrollingVector);
    }
}