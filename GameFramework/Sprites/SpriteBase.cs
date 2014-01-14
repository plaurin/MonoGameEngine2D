using GameFramework.Cameras;
using GameFramework.Scenes;

namespace GameFramework.Sprites
{
    public abstract class SpriteBase
    {
        public string SpriteName { get; set; }

        public Point Position { get; set; }

        public abstract void Draw(DrawContext drawContext, Camera camera, Point mapOffset, Vector parallaxScrollingVector);

        public abstract HitBase GetHit(Point position, Camera camera, Point mapOffset, Vector parallaxScrollingVector);
    }
}