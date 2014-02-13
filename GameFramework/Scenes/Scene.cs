using System;
using System.Collections.Generic;
using System.Linq;

using GameFramework.Cameras;
using GameFramework.Layers;

namespace GameFramework.Scenes
{
    public class Scene
    {
        private readonly List<ILayer> layers;

        public Scene(string name)
        {
            this.Name = name;
            this.layers = new List<ILayer>();
        }

        public string Name { get; private set; }

        public int TotalElements
        {
            get { return this.layers.Sum(l => l.TotalElements); }
        }

        public int DrawnElementsLastFrame
        {
            get { return this.layers.Sum(l => l.DrawnElementsLastFrame); }
        }

        public IEnumerable<ILayer> Layers
        {
            get
            {
                return this.layers;
            }
        }

        public void AddLayer(ILayer layer)
        {
            this.layers.Add(layer);
        }

        public void Draw(DrawContext drawContext, Camera camera)
        {
            foreach (var layer in this.layers)
            {
                if (layer.IsVisible)
                    layer.Draw(drawContext, camera);
            }
        }

        public IEnumerable<HitBase> GetHits(Vector position, Camera camera)
        {
            return this.layers
                .Select(layer => layer.GetHit(position, camera))
                .Where(hit => hit != null);
        }
    }
}