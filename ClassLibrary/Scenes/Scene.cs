using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

using ClassLibrary.Cameras;
using ClassLibrary.Drawing;
using ClassLibrary.Hexes;
using ClassLibrary.Maps;
using ClassLibrary.Sprites;
using ClassLibrary.Tiles;

namespace ClassLibrary.Scenes
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

        public void Save(string path)
        {
            var document = new XDocument();
            
            document.Add(new XElement("Scene",
                new XAttribute("name", this.Name),
                this.maps.Select(m => m.ToXml())));

            document.Save(path);
        }

        public static Scene LoadFrom(GameResourceManager gameResourceManager, string path)
        {
            var document = XDocument.Load(path);
            
            var sceneElement = document.Element("Scene");
            var scene = new Scene(sceneElement.Attribute("name").Value);

            foreach (var mapElement in sceneElement.Elements())
            {
                switch (mapElement.Name.ToString())
                {
                    case "ImageMap":
                        scene.AddMap(ImageMap.FromXml(gameResourceManager, mapElement));
                        break;
                    case "HexMap":
                        scene.AddMap(HexMap.FromXml(gameResourceManager, mapElement));
                        break;
                    case "TileMap":
                        scene.AddMap(TileMap.FromXml(gameResourceManager, mapElement));
                        break;
                    case "ColorMap":
                        scene.AddMap(ColorMap.FromXml(gameResourceManager, mapElement));
                        break;
                    case "SpriteMap":
                        scene.AddMap(SpriteMap.FromXml(gameResourceManager, mapElement));
                        break;
                    case "DrawingMap":
                        scene.AddMap(DrawingMap.FromXml(gameResourceManager, mapElement));
                        break;
                }
            }

            return scene;
        }
    }
}