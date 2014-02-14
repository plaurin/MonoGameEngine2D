using GameFramework.Cameras;
using GameFramework.Scenes;

namespace GameFramework
{
    public interface IHitTarget
    {
        HitBase GetHit(Vector position, ICamera camera, WorldTransform worldTransform);
    }
}