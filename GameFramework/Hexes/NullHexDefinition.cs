using System;

namespace GameFramework.Hexes
{
    public class NullHexDefinition : HexDefinition
    {
        private NullHexDefinition()
            : base(new NullHexSheet(), "null", Rectangle.Empty)
        {
            this.Sheet.AddHexDefinition(this);
        }

        public virtual bool ShouldDraw
        {
            get { return false; }
        }

        public static NullHexDefinition CreateInstance()
        {
            return new NullHexDefinition();
        }
    }
}