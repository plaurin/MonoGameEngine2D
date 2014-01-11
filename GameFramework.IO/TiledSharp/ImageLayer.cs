// Modified for MonoGameEngine2D by Pascal Laurin @2014
// Originals can be found at https://github.com/marshallward/TiledSharp
// Original notice below:
// Distributed as part of TiledSharp, Copyright 2012 Marshall Ward
// Licensed under the Apache License, Version 2.0
// http://www.apache.org/licenses/LICENSE-2.0

using System;
using System.Xml.Linq;

namespace GameFramework.IO.TiledSharp
{
    internal class TmxImageLayer : ITmxElement
    {
        internal TmxImageLayer(XElement imageLayer, string tmxDir = "")
        {
            this.Name = (string)imageLayer.Attribute("name");
            this.Width = (int)imageLayer.Attribute("width");
            this.Height = (int)imageLayer.Attribute("height");
            this.Visible = (bool?)imageLayer.Attribute("visible") ?? true;
            this.Opacity = (double?)imageLayer.Attribute("opacity") ?? 1.0;
            this.Image = new TmxImage(imageLayer.Element("image"), tmxDir);
            this.Properties = new PropertyDict(imageLayer.Element("properties"));
        }

        public string Name { get; private set; }

        public int Width { get; private set; }

        public int Height { get; private set; }

        public bool Visible { get; private set; }

        public double Opacity { get; private set; }

        public TmxImage Image { get; private set; }

        public PropertyDict Properties { get; private set; }
    }
}
