using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WindowsGame1.Hexes
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

        public override void Draw(SpriteBatch spriteBatch, Rectangle destination)
        {
            // Do nothing: Null Object Pattern
        }
    }
}