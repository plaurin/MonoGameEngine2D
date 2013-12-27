using System;

using GameFramework;
using GameFramework.Drawing;

using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Texture = GameFramework.Texture;

namespace MonoGameImplementation.EngineImplementation
{
    public class XnaGameResourceManager : GameResourceManager
    {
        private readonly ContentManager contentManager;

        public XnaGameResourceManager(ContentManager contentManager)
        {
            this.contentManager = contentManager;
        }

        protected override Texture CreateTexture(string assetName)
        {
            return new XnaTexture(this.contentManager.Load<Texture2D>(assetName))
            {
                Name = assetName
            };
        }

        protected override DrawingFont CreateDrawingFont(string assetName)
        {
            return new XnaDrawingFont
            {
                Name = assetName,
                Font = this.contentManager.Load<SpriteFont>(assetName)
            };
        }
    }
}