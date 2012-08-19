using System;

using ClassLibrary.Drawing;
using ClassLibrary.Maps;
using ClassLibrary.Tiles;

namespace ClassLibrary
{
    public abstract class Factory
    {
        public abstract ColorMap CreateColorMap(string name, Color color);

        public abstract ImageMap CreateImageMap(string name, Texture getTexture, Rectangle parseRectangle);

        public abstract TileMap CreateTileMap(string name, Size mapSize, Size tileSize);
    }
}