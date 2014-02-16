using System;

namespace GameFramework.Hexes
{
    public class NullHexDefinition : HexDefinition
    {
        private NullHexDefinition()
            : base(new NullHexSheet(), "null", RectangleInt.Empty)
        {
            this.Sheet.AddHexDefinition(this);
        }

        public override bool ShouldDraw
        {
            get { return false; }
        }

        public static NullHexDefinition CreateInstance()
        {
            return new NullHexDefinition();
        }
    }
}