using System;
using System.Collections.Generic;
using System.Linq;

using GameFramework.Cameras;
using GameFramework.Maps;
using GameFramework.Scenes;

namespace GameFramework.Drawing
{
    public class DrawingMap : MapBase
    {
        private readonly GameResourceManager gameResourceManager;

        private readonly List<DrawingElementBase> elements;

        public DrawingMap(string name, GameResourceManager gameResourceManager)
            : base(name)
        {
            this.gameResourceManager = gameResourceManager;

            this.elements = new List<DrawingElementBase>();
        }

        public IEnumerable<DrawingElementBase> Elements
        {
            get { return this.elements; }
        }

        public TextElement AddText(DrawingFont drawingFont, string text, Vector vector, Color color)
        {
            var textElement = new TextElement(drawingFont, text, vector, color);
            this.elements.Add(textElement);

            return textElement;
        }

        public LineElement AddLine(Vector fromVector, Vector toVector, int width, Color color)
        {
            var lineElement = new LineElement(fromVector, toVector, width, color);
            this.elements.Add(lineElement);

            return lineElement;
        }

        public PolygonElement AddPolygone(int width, Color color, IEnumerable<Vector> vertices)
        {
            var polygonElement = new PolygonElement(vertices, width, color);
            this.elements.Add(polygonElement);

            return polygonElement;
        }

        // Todo: Should be on Extension class as Extension Method
        public RectangleElement AddRectangle(float x, float y, float width, float height, int lineWidth, Color color)
        {
            var rectangleElement = new RectangleElement(x, y, width, height, lineWidth, color);
            this.elements.Add(rectangleElement);

            return rectangleElement;
        }

        public void AddElement(DrawingElementBase element)
        {
            this.elements.Add(element);
        }

        public void ClearAll()
        {
            this.elements.Clear();
        }

        public override void Draw(DrawContext drawContext, Camera camera)
        {
            foreach (var element in this.elements)
            {
                element.Draw(drawContext, camera, this);
            }
        }

        public override HitBase GetHit(Point position, Camera camera)
        {
            return this.elements
                .Select(element => element.GetHit(position, camera, this.Offset, this.ParallaxScrollingVector))
                .FirstOrDefault(spriteHit => spriteHit != null);
        }

        //public override XElement ToXml()
        //{
        //    return XmlRepository.ToXml(this);
        //    //return new XElement("DrawingMap", 
        //    //    this.BaseToXml(),
        //    //    new XElement("Elements", 
        //    //        this.elements.Select(e => e.ToXml())));
        //}
    }
}
