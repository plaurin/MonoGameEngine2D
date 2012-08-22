using System;

using Microsoft.Xna.Framework.Graphics;

using Texture = ClassLibrary.Texture;

namespace WindowsGame1.EngineImplementation
{
    internal class XnaTexture : Texture
    {
        public XnaTexture(Texture2D texture2D)
        {
            this.Texture2D = texture2D;
        }

        public Texture2D Texture2D { get; private set; }
    }
}