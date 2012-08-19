using ClassLibrary;
using ClassLibrary.Hexes;
using ClassLibrary.Sprites;
using ClassLibrary.Tiles;

namespace Editor
{
    using System;

    public class Bootstrapper
    {
        public void Init()
        {
            var gameResourceManager = ServiceLocator.GameResourceManager;

            gameResourceManager.AddTexture(new WinTexture("HexSheet",
                @"C:\Users\Pascal\Dev\DotNet\GitHub\XNAGameEngine2D\GameEngineContent\HexSheet.png"));

            gameResourceManager.AddTexture(new WinTexture("LinkSheet",
                @"C:\Users\Pascal\Dev\DotNet\GitHub\XNAGameEngine2D\GameEngineContent\xander_link_sheet.png"));

            gameResourceManager.AddTexture(new WinTexture("TileSheet",
                @"C:\Users\Pascal\Dev\DotNet\GitHub\XNAGameEngine2D\GameEngineContent\Tiles.png"));

            // TODO: Load from file
            this.CreateSheets(gameResourceManager);
        }

        private void CreateSheets(GameResourceManager gameResourceManager)
        {
            var texture = gameResourceManager.GetTexture("HexSheet");
            var sheet = new HexSheet(texture, "Hexes", new Size(68, 60));
            sheet.CreateHexDefinition("red", new Point(55, 30));
            sheet.CreateHexDefinition("yellow", new Point(163, 330));
            sheet.CreateHexDefinition("purple", new Point(488, 330));
            gameResourceManager.AddHexSheet(sheet);

            texture = gameResourceManager.GetTexture("TileSheet");
            var tileSheet = new TileSheet(texture, "Background", new Size(16, 16));
            tileSheet.CreateTileDefinition("red", new Point(0, 0));
            tileSheet.CreateTileDefinition("green", new Point(16, 0));
            tileSheet.CreateTileDefinition("yellow", new Point(32, 0));
            tileSheet.CreateTileDefinition("purple", new Point(0, 16));
            tileSheet.CreateTileDefinition("orange", new Point(16, 16));
            tileSheet.CreateTileDefinition("blue", new Point(32, 16));
            gameResourceManager.AddTileSheet(tileSheet);

            texture = gameResourceManager.GetTexture("LinkSheet");
            var spriteSheet = new SpriteSheet(texture, "Link");
            spriteSheet.CreateSpriteDefinition("Link01", new Rectangle(3, 3, 16, 22));
            spriteSheet.CreateSpriteDefinition("Sleep01", new Rectangle(45, 219, 32, 40));

            gameResourceManager.AddSpriteSheet(spriteSheet);
        }
    }
}
