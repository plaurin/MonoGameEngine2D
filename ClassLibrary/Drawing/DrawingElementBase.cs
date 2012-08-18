using System;
using System.Xml.Linq;

using ClassLibrary.Cameras;

namespace ClassLibrary.Drawing
{
    public abstract class DrawingElementBase
    {
        public abstract void Draw(DrawContext drawContext, Camera camera, DrawingMap drawingMap);

        public abstract XElement ToXml();
    }
}