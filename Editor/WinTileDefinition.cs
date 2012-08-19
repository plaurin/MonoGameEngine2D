using System;

using ClassLibrary;
using ClassLibrary.Tiles;

namespace Editor
{
    public class WinTileDefinition : TileDefinition
    {
        public WinTileDefinition(TileSheet tileSheet, string tileName, Rectangle rectangle)
            : base(tileSheet, tileName, rectangle)
        {
        }

        //public override void Draw(DrawContext drawContext, Rectangle destination)
        //{
        //    throw new NotImplementedException();
        //}
    }
}