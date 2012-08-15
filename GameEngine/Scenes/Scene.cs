using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

using Microsoft.Xna.Framework.Graphics;

using WindowsGame1.Cameras;
using WindowsGame1.Drawing;
using WindowsGame1.Hexes;
using WindowsGame1.Maps;
using WindowsGame1.Sprites;
using WindowsGame1.Tiles;

namespace WindowsGame1.Scenes
{
    public class Scene
    {
        private readonly List<MapBase> maps;

        public Scene()
        {
            this.maps = new List<MapBase>();
        }

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

        public void Draw(SpriteBatch spriteBatch, Camera camera)
        {
            foreach (var map in this.maps)
            {
                map.Draw(spriteBatch, camera);
            }
        }

        public void Save(string path)
        {
            var document = new XDocument();
            
            document.Add(new XElement("Scene",
                this.maps.Select(m => m.ToXml())));

            document.Save(path);
        }

        public static Scene LoadFrom(GameResourceManager gameResourceManager, string path)
        {
            var scene = new Scene();
            var document = XDocument.Load(path);

            foreach (var mapElement in document.Element("Scene").Elements())
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