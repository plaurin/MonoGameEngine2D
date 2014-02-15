using System;

using GameFramework;
using GameFramework.Drawing;

using Microsoft.Xna.Framework.Graphics;

namespace MonoGameImplementation.EngineImplementation
{
    public class XnaDrawingFont : DrawingFont
    {
        public SpriteFont Font { get; set; }

        public override Vector MeasureString(string text)
        {
            return this.Font.MeasureString(text).ToVector();
        }
    }
}