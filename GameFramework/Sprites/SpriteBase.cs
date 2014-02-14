using GameFramework.Cameras;
using GameFramework.Scenes;

namespace GameFramework.Sprites
{
    public abstract class SpriteBase
    {
        protected SpriteBase()
        {
            this.Origin = Vector.Zero;
        }

        public string SpriteName { get; set; }

        public Vector Position { get; set; }

        public float Rotation { get; set; }

        public Vector Origin { get; set; }

        public abstract int Draw(IDrawContext drawContext, Vector layerOffset,
            Vector parallaxScrollingVector, CameraMode cameraMode);

        public abstract HitBase GetHit(Vector position, Camera camera, Vector layerOffset, Vector parallaxScrollingVector);
    }
}