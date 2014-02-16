using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using GameFramework.Cameras;
using GameFramework.Drawing;
using GameFramework.Hexes;
using GameFramework.Layers;
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
                scene.Children.OfType<ILayer>().Select(XmlRepository.ToXml)));

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

            foreach (var layerElement in sceneElement.Elements())
            {
                switch (layerElement.Name.ToString())
                {
                    case "ImageLayer":
                        scene.Add(ImageXmlRepository.ImageLayerFromXml(gameResourceManager, layerElement));
                        break;
                    case "HexLayer":
                        scene.Add(HexXmlRepository.HexLayerFromXml(gameResourceManager, layerElement));
                        break;
                    case "TileLayer":
                        scene.Add(TileXmlRepository.TileLayerFromXml(gameResourceManager, layerElement));
                        break;
                    case "ColorLayer":
                        scene.Add(ColorXmlRepository.ColorLayerFromXml(layerElement));
                        break;
                    case "SpriteLayer":
                        scene.Add(SpriteXmlRepository.SpriteLayerFromXml(gameResourceManager, layerElement));
                        break;
                    case "DrawingLayer":
                        scene.Add(DrawingXmlRepository.DrawingLayerFromXml(layerElement));
                        break;
                }
            }

            return scene;
        }

        public static XElement ToXml(ILayer layerBase)
        {
            var drawingLayer = layerBase as DrawingLayer;
            if (drawingLayer != null) return DrawingXmlRepository.ToXml(drawingLayer);

            var hexLayer = layerBase as HexLayer;
            if (hexLayer != null) return HexXmlRepository.ToXml(hexLayer);

            var colorLayer = layerBase as ColorLayer;
            if (colorLayer != null) return ColorXmlRepository.ToXml(colorLayer);

            var imageLayer = layerBase as ImageLayer;
            if (imageLayer != null) return ImageXmlRepository.ToXml(imageLayer);

            var spriteLayer = layerBase as SpriteLayer;
            if (spriteLayer != null) return SpriteXmlRepository.ToXml(spriteLayer);

            var tileLayer = layerBase as TileLayer;
            if (tileLayer != null) return TileXmlRepository.ToXml(tileLayer);

            throw new NotSupportedException(layerBase.GetType() + " is not supported");
        }

        internal static IEnumerable<object> LayerBaseToXml(ILayer layerBase)
        {
            yield return new XAttribute("name", layerBase.Name);
            yield return new XAttribute("parallaxScrollingVector", layerBase.ParallaxScrollingVector);
            yield return new XAttribute("offset", layerBase.Offset);
            yield return new XAttribute("cameraMode", layerBase.CameraMode);
        }

        internal static void BaseFromXml(ILayer layerBase, XElement element)
        {
            layerBase.Name = element.Attribute("name").Value;
            layerBase.ParallaxScrollingVector = MathUtil.ParseVector(element.Attribute("parallaxScrollingVector").Value);
            layerBase.Offset = MathUtil.ParseVector(element.Attribute("offset").Value);
            layerBase.CameraMode = (CameraMode)Enum.Parse(typeof(CameraMode), element.Attribute("cameraMode").Value);
        }
    }
}
