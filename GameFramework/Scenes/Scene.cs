using System;
using System.Collections.Generic;
using System.Linq;

using GameFramework.Cameras;
using GameFramework.Layers;

namespace GameFramework.Scenes
{
    public class Scene
    {
        private readonly List<LayerBase> layers;

        public Scene(string name)
        {
            this.Name = name;
            this.layers = new List<LayerBase>();
        }

        public string Name { get; private set; }

        public IEnumerable<LayerBase> Layers
        {
            get
            {
                return this.layers;
            }
        }

        public void AddLayer(LayerBase layer)
        {
            this.layers.Add(layer);
        }

        public void Draw(DrawContext drawContext, Camera camera)
        {
            foreach (var layer in this.layers)
            {
                layer.Draw(drawContext, camera);
            }
        }

        public IEnumerable<HitBase> GetHits(Point position, Camera camera)
        {
            return this.layers
                .Select(layer => layer.GetHit(position, camera))
                .Where(hit => hit != null);
        }
    }
}