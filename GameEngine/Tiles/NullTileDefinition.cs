using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WindowsGame1.Tiles
{
    public class NullTileDefinition : TileDefinition
    {
        private NullTileDefinition()
            : base(new NullTileSheet(), "null", Rectangle.Empty)
        {
        }

        public static NullTileDefinition CreateInstance()
        {
            return new NullTileDefinition();
        }

        public override void Draw(SpriteBatch spriteBatch, Rectangle destination)
        {
            // Do nothing: Null Object Pattern
        }
    }
}