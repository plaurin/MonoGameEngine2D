using System;
using System.Collections.Generic;
using System.Linq;

using GameFramework.Cameras;
using GameFramework.Maps;

namespace GameFramework.Scenes
{
    public class Scene
    {
        private readonly List<MapBase> maps;

        public Scene(string name)
        {
            this.Name = name;
            this.maps = new List<MapBase>();
        }

        public string Name { get; private set; }

        public IEnumerable<MapBase> Maps
        {
            get
            {
                return this.maps;
            }
        }

        public void AddMap(MapBase map)
        {
            this.maps.Add(map);
        }

        public void Draw(DrawContext drawContext, Camera camera)
        {
            foreach (var map in this.maps)
            {
                map.Draw(drawContext, camera);
            }
        }

        public IEnumerable<HitBase> GetHits(Point position, Camera camera)
        {
            return this.maps
                .Select(map => map.GetHit(position, camera))
                .Where(hit => hit != null);
        }
    }
}