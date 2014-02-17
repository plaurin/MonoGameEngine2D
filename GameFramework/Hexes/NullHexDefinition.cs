using System;

namespace GameFramework.Hexes
{
    public class NullHexDefinition : HexDefinition
    {
        private NullHexDefinition()
            : base(new NullHexSheet(), "NullHexDefinition", RectangleInt.Empty)
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