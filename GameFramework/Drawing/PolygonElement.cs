using System;
using System.Collections.Generic;

using GameFramework.Cameras;
using GameFramework.Scenes;

namespace GameFramework.Drawing
{
    public class PolygonElement : DrawingElementBase, IHitTarget
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

        public override void Draw(IDrawContext drawContext, DrawingLayer drawingLayer)
        {
            this.Vertices.ForEachPair((x, y) => this.DrawLine(drawContext, drawingLayer, x, y, this.Width, this.Color));
        }

        public HitBase GetHit(Vector position, ICamera camera, WorldTransform worldTransform)
        {
            // To be implemented
            return null;
        }
    }
}