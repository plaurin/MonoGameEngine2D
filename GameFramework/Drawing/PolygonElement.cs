using System;
using System.Collections.Generic;

using GameFramework.Cameras;
using GameFramework.Scenes;

namespace GameFramework.Drawing
{
    public class PolygonElement : DrawingElementBase
    {
        private readonly List<Vector> vertices;
        
        public PolygonElement(IEnumerable<Vector> vertices, int width, Color color)
        {
            this.vertices = new List<Vector>(vertices);
            this.Width = width;
            this.Color = color;
        }

        public int Width { get; private set; }

        public Color Color { get; private set; }

        public IEnumerable<Vector> Vertices
        {
            get { return this.vertices; }
        }

        public override void Draw(DrawContext drawContext, Camera camera, DrawingLayer drawingLayer)
        {
            this.Vertices.ForEachPair((x, y) => this.DrawLine(drawContext, camera, drawingLayer, x, y, this.Width, this.Color));
        }

        public override HitBase GetHit(Point position, Camera camera, Point layerOffset, Vector parallaxScrollingVector)
        {
            // To be implemented
            return null;
        }
    }
}