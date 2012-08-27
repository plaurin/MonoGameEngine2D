namespace Editor
{
    using System;

    public class Bootstrapper
    {
        public void Init()
        {
            var gameResourceManager = ServiceLocator.GameResourceManager;

            gameResourceManager.DeclareTexturePath("HexSheet",
                @"C:\Users\Pascal\Dev\DotNet\GitHub\XNAGameEngine2D\GameEngineContent\HexSheet.png");

            gameResourceManager.DeclareTexturePath("LinkSheet",
                @"C:\Users\Pascal\Dev\DotNet\GitHub\XNAGameEngine2D\GameEngineContent\xander_link_sheet.png");

            gameResourceManager.DeclareTexturePath("TileSheet",
                @"C:\Users\Pascal\Dev\DotNet\GitHub\XNAGameEngine2D\GameEngineContent\Tiles.png");
        }
    }
}
