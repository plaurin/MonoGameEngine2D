using System.Xml.Linq;
using GameFramework.Layers;

namespace GameFramework.IO.Repositories
{
    public class ImageXmlRepository
    {
        public static XElement ToXml(ImageLayer imageLayer)
        {
            return new XElement("ImageLayer",
                new XAttribute("name", imageLayer.Name),
                new XElement("Texture", imageLayer.Texture.Name),
                new XElement("Rectangle", imageLayer.Rectangle));
        }

        public static ImageLayer ImageLayerFromXml(GameResourceManager gameResourceManager, XElement layerElement)
        {
            var name = layerElement.Attribute("name").Value;
            var textureName = layerElement.Element("Texture").Value;
            var rectangleValue = layerElement.Element("Rectangle").Value;

            return new ImageLayer(name, gameResourceManager.GetTexture(textureName), MathUtil.ParseRectangle(rectangleValue));
        }
    }
}