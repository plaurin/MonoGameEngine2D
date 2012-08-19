using System;

using ClassLibrary;
using ClassLibrary.Maps;
using ClassLibrary.Tiles;

namespace Editor
{
    public class WinFactory : Factory
    {
        public override ColorMap CreateColorMap(string name, Color color)
        {
            return new WinColorMap(name, color);
        }

        public override ImageMap CreateImageMap(string name, Texture texture, Rectangle rectangle)
        {
            return new WinImageMap(name, texture, rectangle);
        }

        public override TileMap CreateTileMap(string name, Size mapSize, Size tileSize)
        {
            return new WinTileMap(name, mapSize, tileSize);
        }
    }
}