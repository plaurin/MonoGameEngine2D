using System;
using System.Collections.Generic;
using System.Linq;

using GameFramework.Cameras;
using GameFramework.Layers;

namespace GameFramework.Scenes
{
    public class Scene : IComposite, IUpdatable, IDrawable, INavigatorMetadataProvider
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

        public IEnumerable<HitBase> GetHits(Vector position, ICamera camera)
        {
            return this.Children.OfType<IHitTarget>()
                .Select(hitTarget => hitTarget.GetHit(position, camera, WorldTransform.New))
                .Where(hit => hit != null);
        }

        public void Update(IGameTiming gameTiming)
        {
            foreach (var updatable in this.sceneObjects.OfType<IUpdatable>())
                updatable.Update(gameTiming);
        }

        public int Draw(IDrawContext drawContext)
        {
            return this.sceneObjects.OfType<IDrawable>()
                .Sum(drawable => drawable.Draw(drawContext));
        }

        public NavigatorMetadata GetMetadata()
        {
            return new NavigatorMetadata(this.Name, NodeKind.Scene);
        }
    }
}