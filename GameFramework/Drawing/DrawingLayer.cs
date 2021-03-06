﻿using System;
using System.Collections.Generic;
using System.Linq;

using GameFramework.Cameras;
using GameFramework.Layers;
using GameFramework.Scenes;

namespace GameFramework.Drawing
{
    public class DrawingLayer : LayerBase, IComposite, IHitTarget
    {
        private readonly List<DrawingElementBase> elements;

        public DrawingLayer(string name)
            : base(name)
        {
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

        public RectangleElement AddRectangle(Rectangle rectangle, int lineWidth, Color color)
        {
            var rectangleElement = new RectangleElement(rectangle, lineWidth, color);
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

        public override int TotalElements
        {
            get { return this.elements.Count; }
        }

        public override int DrawnElementsLastFrame
        {
            get { return this.TotalElements; }
        }

        public override int Draw(IDrawContext drawContext, Transform transform)
        {
            this.SetSampler(drawContext);

            var total = 0;

            // TODO: Optimize filter element outside the view should not be drawn
            foreach (var element in this.elements)
            {
                if (element.IsVisible)
                {
                    element.Draw(drawContext, this);
                    total++;
                }
            }

            return total;
        }

        public IEnumerable<object> Children
        {
            get { return this.elements; }
        }

        public HitBase GetHit(Vector position, ICamera camera, WorldTransform worldTransform)
        {
            var newTransform = worldTransform.Compose(this.Offset, this.ParallaxScrollingVector);

            return this.elements.OfType<IHitTarget>()
                .Select(element => element.GetHit(position, camera, newTransform))
                .FirstOrDefault(spriteHit => spriteHit != null);
        }
    }
}
