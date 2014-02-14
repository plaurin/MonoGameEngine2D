using System;
using System.Collections.Generic;
using System.Linq;

using GameFramework.Cameras;
using GameFramework.Layers;

namespace GameFramework.Scenes
{
    public class Scene : IComposite, IUpdatable, IDrawable
    {
        private readonly List<object> sceneObjects;

        public Scene(string name)
        {
            this.Name = name;
            this.sceneObjects = new List<object>();
        }

        public string Name { get; private set; }

        public int TotalElements
        {
            get { return this.Children.OfType<ILayer>().Sum(l => l.TotalElements); }
        }

        public int DrawnElementsLastFrame
        {
            get { return this.Children.OfType<ILayer>().Sum(l => l.DrawnElementsLastFrame); }
        }

        public IEnumerable<object> Children
        {
            get { return this.sceneObjects; }
        }

        public void Add(object sceneObject)
        {
            this.sceneObjects.Add(sceneObject);
        }

        public IEnumerable<HitBase> GetHits(Vector position, Camera camera)
        {
            return this.Children.OfType<ILayer>()
                .Select(layer => layer.GetHit(position, camera))
                .Where(hit => hit != null);
        }

        public void Update(IGameTiming gameTiming)
        {
            foreach (var updatable in this.sceneObjects.OfType<IUpdatable>())
                updatable.Update(gameTiming);
        }

        public int Draw(DrawContext drawContext)
        {
            return this.sceneObjects.OfType<IDrawable>()
                .Sum(drawable => drawable.Draw(drawContext));
        }
    }
}