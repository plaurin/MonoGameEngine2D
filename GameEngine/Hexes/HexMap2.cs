using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using WindowsGame1.Sprites;

namespace WindowsGame1.Hexes
{
    public class HexMap2
    {
        private readonly HexDefinition[,] map;

        public HexMap2(Size mapSize, Size hexSize, HexDefinition defaultHexDefinition = null)
        {
            this.MapSize = mapSize;
            this.HexSize = hexSize;

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

        public void Draw(SpriteBatch spriteBatch)
        {
            for (var i = 0; i < this.MapSize.Width; i++)
                for (var j = 0; j < this.MapSize.Height; j++)
                {
                    var destination = new Rectangle(i * this.HexSize.Width, j * this.HexSize.Height,
                        this.HexSize.Width, this.HexSize.Height);

                    this.map[i, j].Draw(spriteBatch, destination);
                }
        }
    }
}