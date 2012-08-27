using System;
using System.Collections.Generic;

using ClassLibrary;

namespace WPFGameLibrary.EngineImplementation
{
    public class WpfGameResourceManager : GameResourceManager
    {
        private readonly Dictionary<string, string> texturePath;

        public WpfGameResourceManager()
        {
            this.texturePath = new Dictionary<string, string>();
        }

        public void DeclareTexturePath(string assetName, string path)
        {
            this.texturePath.Add(assetName, path);
        }

        protected override Texture CreateTexture(string assetName)
        {
            var path = this.texturePath[assetName];
            return new WpfTexture(assetName, path)
            {
                Name = assetName
            };
        }

        protected override ClassLibrary.Drawing.DrawingFont CreateDrawingFont(string assetName)
        {
            return new WpfDrawingFont
            {
                Name = assetName
            };
        }
    }
}