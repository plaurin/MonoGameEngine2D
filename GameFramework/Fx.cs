using System;
using System.Collections.Generic;

namespace FxBase
{
    [Obsolete("To delete!")]
    public abstract class Scene
    {
        protected Scene()
        {
            this.Entities = new List<Entity>();
        }

        public IList<Entity> Entities { get; private set; }
    }

    [Obsolete("To delete!")]
    public abstract class Entity
    {
    }

    [Obsolete("To delete!")]
    public class Sprite : Entity
    {
        public Vector2 Position { get; set; }
        public string TexturePath { get; set; }
    }

    [Obsolete("To delete!")]
    public class Line : Entity
    {
        public Vector2 Point1 { get; set; }
        public Vector2 Point2 { get; set; }
    }

    [Obsolete("To delete!")]
    public class Vector2
    {
        public Vector2()
        {
        }

        public Vector2(float x, float y)
        {
            this.X = x;
            this.Y = y;
        }
        public float X { get; set; }
        public float Y { get; set; }
    }
}
