using System;

using GameFramework.Cameras;
using GameFramework.Scenes;

namespace GameFramework.Maps
{
    public abstract class MapBase
    {
        protected MapBase(string name)
        {
            this.Name = name;
            this.CameraMode = CameraMode.Follow;
            this.ParallaxScrollingVector = Vector.One;
        }

        public string Name { get; internal set; }

        public CameraMode CameraMode { get; set; }

        public Vector ParallaxScrollingVector { get; set; }
        
        public Point Offset { get; set; }

        public abstract void Draw(DrawContext drawContext, Camera camera);

        public virtual HitBase GetHit(Point position, Camera camera)
        {
            return null;
        }
    }
}
