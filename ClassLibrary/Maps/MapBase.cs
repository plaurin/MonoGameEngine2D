using System;
using System.Collections.Generic;
using System.Xml.Linq;

using ClassLibrary.Cameras;

namespace ClassLibrary.Maps
{
    public abstract class MapBase
    {
        protected MapBase(string name)
        {
            this.Name = name;
            this.CameraMode = CameraMode.Follow;
            this.ParallaxScrollingVector = Vector.One;
        }

        public string Name { get; private set; }

        public CameraMode CameraMode { get; set; }

        public Vector ParallaxScrollingVector { get; set; }

        public abstract void Draw(DrawContext drawContext, Camera camera);

        public abstract XElement ToXml();

        public IEnumerable<object> BaseToXml()
        {
            yield return new XAttribute("name", this.Name);
            yield return new XAttribute("parallaxScrollingVector", this.ParallaxScrollingVector);
            yield return new XAttribute("cameraMode", this.CameraMode);
        }

        public void BaseFromXml(XElement element)
        {
            this.Name = element.Attribute("name").Value;
            this.ParallaxScrollingVector = MathUtil.ParseVector(element.Attribute("parallaxScrollingVector").Value);
            this.CameraMode = (CameraMode)Enum.Parse(typeof(CameraMode), element.Attribute("cameraMode").Value);
        }
    }
}
