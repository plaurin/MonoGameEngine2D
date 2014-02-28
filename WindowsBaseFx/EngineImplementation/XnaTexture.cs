using System;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameImplementation.EngineImplementation
{
    internal class XnaTexture : GameFramework.Texture
    {
        public XnaTexture(Texture2D texture2D)
        {
            this.Texture2D = texture2D;
        }

        public Texture2D Texture2D { get; private set; }
    }
}