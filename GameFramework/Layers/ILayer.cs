using GameFramework.Cameras;
using GameFramework.Scenes;

namespace GameFramework.Layers
{
    public interface ILayer
    {
        string Name { get; set; }

        CameraMode CameraMode { get; set; }

        Vector ParallaxScrollingVector { get; set; }

        Vector Offset { get; set; }

        bool IsVisible { get; set; }

        int TotalElements { get; }

        int DrawnElementsLastFrame { get; }

        void Draw(DrawContext drawContext, Camera camera);

        HitBase GetHit(Vector position, Camera camera);
    }
}