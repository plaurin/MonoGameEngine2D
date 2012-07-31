using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WindowsGame1.Hexes
{
    public class NullHexDefinition : HexDefinition
    {
        private NullHexDefinition()
            : base(null, "null", Rectangle.Empty)
        {
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