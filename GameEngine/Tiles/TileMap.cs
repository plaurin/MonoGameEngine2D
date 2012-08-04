using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using WindowsGame1.Cameras;
using WindowsGame1.Maps;
using WindowsGame1.Sprites;

namespace WindowsGame1.Tiles
{
    public class TileMap : MapBase
    {
        private readonly TileDefinition[,] map;

        public TileMap(Size mapSize, Size tileSize, TileDefinition defaultTileDefinition = null)
        {
            this.MapSize = mapSize;
            this.TileSize = tileSize;

            this.map = new TileDefinition[mapSize.Width, mapSize.Height];
            if (defaultTileDefinition == null)
                defaultTileDefinition = NullTileDefinition.CreateInstance();

            for (var i = 0; i < mapSize.Width; i++)
                for (var j = 0; j < mapSize.Height; j++)
                {
                    this.map[i, j] = defaultTileDefinition;
                }
        }

        public Size MapSize { get; private set; }

        public Size TileSize { get; private set; }

        public TileDefinition this[int x, int y]
        {
            get { return this.map[x, y]; }
            set { this.map[x, y] = value; }
        }

        public override void Draw(SpriteBatch spriteBatch, Camera camera)
        {
            for (var i = 0; i < this.MapSize.Width; i++)
                for (var j = 0; j < this.MapSize.Height; j++)
                {
                    var destination =
                        new Rectangle(i * this.TileSize.Width, j * this.TileSize.Height, this.TileSize.Width, this.TileSize.Height)
                        .Scale(camera.ZoomFactor)
                        .Translate(camera.GetSceneTranslationVector(this.ParallaxScrollingVector));

                    this.map[i, j].Draw(spriteBatch, destination);
                }
        }
    }
}