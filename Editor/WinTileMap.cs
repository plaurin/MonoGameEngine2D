using System;

using ClassLibrary;
using ClassLibrary.Tiles;

namespace Editor
{
    public class WinTileMap : TileMap
    {
        public WinTileMap(string name, Size mapSize, Size tileSize)
            : base(name, mapSize, tileSize)
        {
        }
    }
}