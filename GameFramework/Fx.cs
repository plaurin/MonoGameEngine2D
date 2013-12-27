using System;
using System.Collections.Generic;

namespace FxBase
{
    public abstract class Scene
    {
        protected Scene()
        {
            this.Entities = new List<Entity>();
        }

        public IList<Entity> Entities { get; private set; }
    }

    public abstract class Entity
    {
    }

    public class Sprite : Entity
    {
        public Vector2 Position { get; set; }
        public string TexturePath { get; set; }
    }

    public class Line : Entity
    {
        public Vector2 Point1 { get; set; }
        public Vector2 Point2 { get; set; }
    }

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
