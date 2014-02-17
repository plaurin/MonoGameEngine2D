using GameFramework.Tiles;

namespace GameFramework.IO
{
    public class TiledObject
    {
        public TiledObject(TileDefinition tileDefinition, Vector position)
        {
            this.TileDefinition = tileDefinition;
            this.Position = position;
        }

        public TileDefinition TileDefinition { get; private set; }

        public Vector Position { get; private set; }
    }
}