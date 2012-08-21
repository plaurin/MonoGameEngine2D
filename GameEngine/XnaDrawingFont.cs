using System;

using ClassLibrary;
using ClassLibrary.Drawing;

using Microsoft.Xna.Framework.Graphics;

namespace WindowsGame1
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