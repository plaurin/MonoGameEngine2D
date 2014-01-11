// Modified for MonoGameEngine2D by Pascal Laurin @2014
// Originals can be found at https://github.com/marshallward/TiledSharp
// Original notice below:
// Distributed as part of TiledSharp, Copyright 2012 Marshall Ward
// Licensed under the Apache License, Version 2.0
// http://www.apache.org/licenses/LICENSE-2.0

using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace GameFramework.IO.TiledSharp
{
    internal class TmxObjectGroup : ITmxElement
    {
        internal TmxObjectGroup(XElement objectGroup)
        {
            this.Name = (string)objectGroup.Attribute("name");
            this.Color = new TmxColor(objectGroup.Attribute("color"));
            this.Opacity = (double?)objectGroup.Attribute("opacity") ?? 1.0;
            this.Visible = (bool?)objectGroup.Attribute("visible") ?? true;

            this.Objects = new TmxList<TmxObject>();
            foreach (var e in objectGroup.Elements("object"))
                this.Objects.Add(new TmxObject(e));

            this.Properties = new PropertyDict(objectGroup.Element("properties"));
        }

        public string Name { get; private set; }

        public TmxColor Color { get; private set; }

        public double Opacity { get; private set; }

        public bool Visible { get; private set; }

        public TmxList<TmxObject> Objects { get; private set; }

        public PropertyDict Properties { get; private set; }

        internal class TmxObject : ITmxElement
        {
            // Many TmxObjectTypes are distinguished by null values in fields
            // It might be smart to subclass TmxObject
            internal TmxObject(XElement objet)
            {
                this.Name = (string)objet.Attribute("name") ?? string.Empty;
                this.Type = (string)objet.Attribute("type");
                this.X = (int)objet.Attribute("x");
                this.Y = (int)objet.Attribute("y");
                this.Visible = (bool?)objet.Attribute("visible") ?? true;
                this.Width = (int?)objet.Attribute("width") ?? 0;
                this.Height = (int?)objet.Attribute("height") ?? 0;
                this.Rotation = (double?)objet.Attribute("rotation") ?? 0.0;

                // Assess object type and assign appropriate content
                var gid = objet.Attribute("gid");
                var ellipse = objet.Element("ellipse");
                var polygon = objet.Element("polygon");
                var polyline = objet.Element("polyline");

                if (gid != null)
                {
                    this.Tile = new TmxLayerTile((uint)gid, this.X, this.Y);
                    this.ObjectType = TmxObjectType.Tile;
                }
                else if (ellipse != null)
                {
                    this.ObjectType = TmxObjectType.Ellipse;
                }
                else if (polygon != null)
                {
                    this.Points = this.ParsePoints(polygon);
                    this.ObjectType = TmxObjectType.Polygon;
                }
                else if (polyline != null)
                {
                    this.Points = this.ParsePoints(polyline);
                    this.ObjectType = TmxObjectType.Polyline;
                }
                else this.ObjectType = TmxObjectType.Basic;

                this.Properties = new PropertyDict(objet.Element("properties"));
            }

            public string Name { get; private set; }

            public TmxObjectType ObjectType { get; private set; }

            public string Type { get; private set; }

            public int X { get; private set; }

            public int Y { get; private set; }

            public int Width { get; private set; }

            public int Height { get; private set; }

            public double Rotation { get; private set; }

            public TmxLayerTile Tile { get; private set; }

            public bool Visible { get; private set; }

            public List<Tuple<int, int>> Points { get; private set; }

            public PropertyDict Properties { get; private set; }

            public List<Tuple<int, int>> ParsePoints(XElement element)
            {
                var points = new List<Tuple<int, int>>();

                var pointString = (string)element.Attribute("points");
                var pointStringPair = pointString.Split(' ');
                foreach (var s in pointStringPair)
                {
                    var pt = s.Split(',');
                    var x = int.Parse(pt[0]);
                    var y = int.Parse(pt[1]);
                    points.Add(Tuple.Create(x, y));
                }

                return points;
            }
        }

        internal enum TmxObjectType : byte
        {
            Basic,
            Tile,
            Ellipse,
            Polygon,
            Polyline
        }
    }
}
