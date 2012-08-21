using System;

namespace ClassLibrary.Drawing
{
    public class DrawingFont
    {
        public string Name { get; set; }

        //public SpriteFont Font { get; set; }
        public virtual Vector MeasureString(string text)
        {
            throw new NotImplementedException();
        }
    }
}