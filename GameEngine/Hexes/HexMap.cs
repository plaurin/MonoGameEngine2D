using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using WindowsGame1.Cameras;
using WindowsGame1.Maps;
using WindowsGame1.Sprites;

namespace WindowsGame1.Hexes
{
    public class HexMap : MapBase
    {
        private readonly HexDefinition[,] map;
        private readonly HexGrid grid;

        public HexMap(Size mapSize, Size hexSize, HexDefinition defaultHexDefinition = null)
        {
            this.MapSize = mapSize;
            this.HexSize = hexSize;

            this.grid = HexGrid.CreateHexMap(hexSize.Width / 2, mapSize.Width);

            this.map = new HexDefinition[mapSize.Width, mapSize.Height];
            if (defaultHexDefinition == null)
                defaultHexDefinition = NullHexDefinition.CreateInstance();

            for (var i = 0; i < mapSize.Width; i++)
                for (var j = 0; j < mapSize.Height; j++)
                {
                    this.map[i, j] = defaultHexDefinition;
                }
        }

        public Size MapSize { get; private set; }

        public Size HexSize { get; private set; }

        public HexDefinition this[int x, int y]
        {
            get { return this.map[x, y]; }
            set { this.map[x, y] = value; }
        }

        public void Draw(SpriteBatch spriteBatch, Camera camera)
        {
            for (var i = 0; i < this.MapSize.Width; i++)
                for (var j = 0; j < this.MapSize.Height; j++)
                {
                    var gridElement = this.grid[i, j];

                    var destination = gridElement.Rectangle
                        .Scale(camera.ZoomFactor)
                        .Translate(camera.GetSceneTranslationVector(this.ParallaxScrollingVector));

                    this.map[i, j].Draw(spriteBatch, destination);
                }
        }
    }
}