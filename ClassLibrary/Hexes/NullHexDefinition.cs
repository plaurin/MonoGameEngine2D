using System;

namespace ClassLibrary.Hexes
{
    public class NullHexDefinition : HexDefinition
    {
        private NullHexDefinition()
            : base(new NullHexSheet(), "null", Rectangle.Empty)
        {
            this.Sheet.AddHexDefinition(this);
        }

        public static NullHexDefinition CreateInstance()
        {
            return new NullHexDefinition();
        }
    }
}