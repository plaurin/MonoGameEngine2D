using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using GameFramework.Cameras;
using GameFramework.Drawing;
using GameFramework.Hexes;
using GameFramework.Maps;
using GameFramework.Scenes;
using GameFramework.Sheets;
using GameFramework.Sprites;
using GameFramework.Tiles;

namespace GameFramework.IO.Repositories
{
    public class XmlRepository
    {
        public void Save(Scene scene, string path)
        {
            var document = new XDocument();

            document.Add(new XElement("Scene",
                new XAttribute("name", scene.Name),
                //scene.Maps.Select(m => m.ToXml())));
                scene.Maps.Select(XmlRepository.ToXml)));

            // TODO: Fix this
            //document.Save(path);
        }

        public void Save(SheetBase sheetBase, string path)
        {
            var document = new XDocument();

            document.Add(SheetXmlRepository.ToXml(sheetBase));

            // TODO: Fix this
            //document.Save(path);
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
                        scene.AddMap(ImageXmlRepository.ImageMapFromXml(gameResourceManager, mapElement));
                        break;
                    case "HexMap":
                        scene.AddMap(HexXmlRepository.HexMapFromXml(gameResourceManager, mapElement));
                        break;
                    case "TileMap":
                        scene.AddMap(TileXmlRepository.TileMapFromXml(gameResourceManager, mapElement));
                        break;
                    case "ColorMap":
                        scene.AddMap(ColorXmlRepository.ColorMapFromXml(mapElement));
                        break;
                    case "SpriteMap":
                        scene.AddMap(SpriteXmlRepository.SpriteMapFromXml(gameResourceManager, mapElement));
                        break;
                    case "DrawingMap":
                        scene.AddMap(DrawingXmlRepository.DrawingMapFromXml(gameResourceManager, mapElement));
                        break;
                }
            }

            return scene;
        }

        public static XElement ToXml(MapBase mapBase)
        {
            var drawingMap = mapBase as DrawingMap;
            if (drawingMap != null) return DrawingXmlRepository.ToXml(drawingMap);

            var hexMap = mapBase as HexMap;
            if (hexMap != null) return HexXmlRepository.ToXml(hexMap);

            var colorMap = mapBase as ColorMap;
            if (colorMap != null) return ColorXmlRepository.ToXml(colorMap);

            var imageMap = mapBase as ImageMap;
            if (imageMap != null) return ImageXmlRepository.ToXml(imageMap);

            var spriteMap = mapBase as SpriteMap;
            if (spriteMap != null) return SpriteXmlRepository.ToXml(spriteMap);

            var tileMap = mapBase as TileMap;
            if (tileMap != null) return TileXmlRepository.ToXml(tileMap);

            throw new NotSupportedException(mapBase.GetType() + " is not supported");
        }

        internal static IEnumerable<object> MapBaseToXml(MapBase mapBase)
        {
            yield return new XAttribute("name", mapBase.Name);
            yield return new XAttribute("parallaxScrollingVector", mapBase.ParallaxScrollingVector);
            yield return new XAttribute("offset", mapBase.Offset);
            yield return new XAttribute("cameraMode", mapBase.CameraMode);
        }

        internal static void BaseFromXml(MapBase mapBase, XElement element)
        {
            mapBase.Name = element.Attribute("name").Value;
            mapBase.ParallaxScrollingVector = MathUtil.ParseVector(element.Attribute("parallaxScrollingVector").Value);
            mapBase.Offset = MathUtil.ParsePoint(element.Attribute("offset").Value);
            mapBase.CameraMode = (CameraMode)Enum.Parse(typeof(CameraMode), element.Attribute("cameraMode").Value);
        }
    }
}
