using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using GameFramework;
using GameFramework.Cameras;
using GameFramework.Drawing;
using GameFramework.Hexes;
using GameFramework.Inputs;
using GameFramework.IO.Repositories;
using GameFramework.Maps;
using GameFramework.Scenes;
using GameFramework.Screens;
using GameFramework.Sheets;
using GameFramework.Sprites;
using GameFramework.Tiles;
using GameFramework.Utilities;

namespace SamplesBrowser.Sandbox
{
    public class SandboxScreen : ScreenBase
    {
        private readonly ScreenNavigation screenNavigation;

        private Camera camera;

        private GameResourceManager gameResourceManager;

        private Point player;

        private float range;

        private Scene scene;

        private MouseStateBase mouseState;

        private InputConfiguration inputConfiguration;

        private MouseCursorMap mouseMap;
        private DiagnosticMap diagnosticMap;

        //private string hits;
        private List<HitBase> hits;

        public SandboxScreen(ScreenNavigation screenNavigation)
        {
            this.screenNavigation = screenNavigation;
        }

        public override void Initialize(Camera theCamera)
        {
            this.camera = theCamera;
            this.player = new Point(25, 25);
            this.range = 0.25f;

            // theCamera.Center = CameraCenter.WindowTopLeft;
        }

        public override InputConfiguration GetInputConfiguration()
        {
            this.inputConfiguration = new InputConfiguration();

            this.inputConfiguration.AddDigitalButton("Back").Assign(KeyboardKeys.Escape)
                .MapClickTo(gt => this.screenNavigation.NavigateBack());

            this.inputConfiguration.AddDigitalButton("Left").Assign(KeyboardKeys.Left).MapTo(gt => this.camera.Move(-60 * gt.ElapsedSeconds, 0));
            this.inputConfiguration.AddDigitalButton("Right").Assign(KeyboardKeys.Right).MapTo(gt => this.camera.Move(60 * gt.ElapsedSeconds, 0));
            this.inputConfiguration.AddDigitalButton("Up").Assign(KeyboardKeys.Up).MapTo(gt => this.camera.Move(0, -60 * gt.ElapsedSeconds));
            this.inputConfiguration.AddDigitalButton("Down").Assign(KeyboardKeys.Down).MapTo(gt => this.camera.Move(0, 60 * gt.ElapsedSeconds));

            this.inputConfiguration.AddDigitalButton("ZoomIn").Assign(KeyboardKeys.A).MapTo(gt => this.camera.ZoomFactor *= 1.2f * (1 + gt.ElapsedSeconds));
            this.inputConfiguration.AddDigitalButton("ZoomOut").Assign(KeyboardKeys.Z).MapTo(gt => this.camera.ZoomFactor *= 1 / (1.2f * (1 + gt.ElapsedSeconds)));

            this.inputConfiguration.AddDigitalButton("RangeUp").Assign(KeyboardKeys.W).MapTo(gt => this.range *= 1.2f * (1 + gt.ElapsedSeconds));
            this.inputConfiguration.AddDigitalButton("RangeDown").Assign(KeyboardKeys.Q).MapTo(gt => this.range *= 1 / (1.2f * (1 + gt.ElapsedSeconds)));

            this.inputConfiguration.AddDigitalButton("Selection").Assign(MouseButtons.Left).MapTo(elapse =>
            {
                this.hits = this.scene.GetHits(this.mouseState.AbsolutePosition, this.camera).ToList();
            });

            this.inputConfiguration.AddMouseTracking(this.camera).OnMove((mt, e) =>
            {
                this.mouseState = mt;
                this.mouseMap.Update(this.mouseState);
            });

            return this.inputConfiguration;
        }

        public override void LoadContent(GameResourceManager theResourceManager)
        {
            this.gameResourceManager = theResourceManager;

            this.gameResourceManager.AddDrawingFont(@"Sandbox\SpriteFont1");
        }

        public override void Update(IGameTiming gameTime)
        {
            var colorMap = this.scene.Maps.OfType<ColorMap>().FirstOrDefault();
            if (colorMap != null)
                colorMap.Color = new Color(255, 0, 0, (int)(255 * Math.Min(this.range, 1.0f)));

            this.diagnosticMap.Update(gameTime, this.camera, this.mouseState, this.hits);
            this.diagnosticMap.UpdateLine("Range", this.range);
        }

        public override Scene GetScene()
        {
            this.scene = new Scene("scene1");

            this.scene.AddMap(this.CreateImageMap());
            this.scene.AddMap(this.CreateHexMap());
            this.scene.AddMap(this.CreateTileMap());
            this.scene.AddMap(this.CreateColorMap());
            this.scene.AddMap(this.CreateHexMapTestDistance());
            this.scene.AddMap(this.CreateSpriteMap());
            this.scene.AddMap(this.CreateDiagnosticMap());
            this.scene.AddMap(this.CreateMouseCursorMap());

            //this.scene.Save(@"C:\Users\Pascal\Dev\DotNet\GitHub\XNAGameEngine2D\TestScene.xml");

            //this.TestSceneSaveLoad();
            //this.TestSheetsSaveLoad();

            return this.scene;
        }

        private void TestSceneSaveLoad()
        {
            var scene2 = XmlRepository.LoadFrom(this.gameResourceManager,
                @"C:\Users\Pascal\Dev\DotNet\GitHub\XNAGameEngine2D\TestScene.xml");

            foreach (var map in this.scene.Maps)
            {
                var otherMap = scene2.Maps.Single(x => x.Name == map.Name);

                var myXml = XmlRepository.ToXml(map); // map.ToXml();
                var otherXml = XmlRepository.ToXml(otherMap); // otherMap.ToXml();

                if (myXml.ToString() != otherXml.ToString())
                {
                    //Debugger.Break();
                }
            }
        }

        private void TestSheetsSaveLoad()
        {
            var otherHexSheet = SheetXmlRepository.LoadFrom<HexSheet>(
                this.gameResourceManager, @"C:\Users\Pascal\Dev\DotNet\GitHub\XNAGameEngine2D\Color HexSheet.xml");

            var myHexSheetXml = SheetXmlRepository.ToXml(this.gameResourceManager.GetHexSheet(otherHexSheet.Name));
            var otherHexSheetXml = SheetXmlRepository.ToXml(otherHexSheet);
            if (myHexSheetXml.ToString() != otherHexSheetXml.ToString()) Debugger.Break();

            var otherTileSheet = SheetXmlRepository.LoadFrom<TileSheet>(
                this.gameResourceManager, @"C:\Users\Pascal\Dev\DotNet\GitHub\XNAGameEngine2D\Color TileSheet.xml");

            var myTileSheetXml = SheetXmlRepository.ToXml(this.gameResourceManager.GetTileSheet(otherTileSheet.Name));
            var otherTileSheetXml = SheetXmlRepository.ToXml(otherTileSheet);
            if (myTileSheetXml.ToString() != otherTileSheetXml.ToString()) Debugger.Break();

            var otherSpriteSheet = SheetXmlRepository.LoadFrom<SpriteSheet>(
                this.gameResourceManager, @"C:\Users\Pascal\Dev\DotNet\GitHub\XNAGameEngine2D\Link SpriteSheet.xml");

            var mySpriteSheetXml = SheetXmlRepository.ToXml(this.gameResourceManager.GetSpriteSheet(otherSpriteSheet.Name));
            var otherSpriteSheetXml = SheetXmlRepository.ToXml(otherSpriteSheet);
            if (mySpriteSheetXml.ToString() != otherSpriteSheetXml.ToString()) Debugger.Break();
        }

        private MapBase CreateMouseCursorMap()
        {
            this.mouseMap = MouseCursorMap.Create(this.gameResourceManager);

            return this.mouseMap;
        }

        private DiagnosticMap CreateDiagnosticMap()
        {
            var font = this.gameResourceManager.GetDrawingFont(@"Sandbox\SpriteFont1"); 
            
            this.diagnosticMap = new DiagnosticMap(this.gameResourceManager, font);
            this.diagnosticMap.AddLine("Range", "Range: {0:f1}");

            return this.diagnosticMap;
        }

        private ImageMap CreateImageMap()
        {
            var texture = this.gameResourceManager.GetTexture(@"Sandbox\LinkSheet");

            return new ImageMap("Image", texture, new Rectangle(10, 10, 250, 250));
        }

        private ColorMap CreateColorMap()
        {
            var alpha = Math.Min(this.range, 1.0f);

            return new ColorMap("Red", new Color(255, 0, 0, (int)(255 * alpha)));
        }

        private HexMap CreateHexMap()
        {
            var texture = this.gameResourceManager.GetTexture(@"Sandbox\HexSheet");
            var sheet = new HexSheet(texture, "Hexes", new Size(68, 60));
            var red = sheet.CreateHexDefinition("red", new Point(55, 30));
            var yellow = sheet.CreateHexDefinition("yellow", new Point(163, 330));
            var purple = sheet.CreateHexDefinition("purple", new Point(488, 330));

            //sheet.Save(@"C:\Users\Pascal\Dev\DotNet\GitHub\XNAGameEngine2D\Color HexSheet.xml");

            this.gameResourceManager.AddHexSheet(sheet);

            var map = new HexMap("Hex", new Size(4, 4), new Size(68, 60), 42);
            map[2, 0] = purple;
            map[2, 1] = purple;
            map[2, 2] = yellow;
            map[0, 1] = red;
            map[1, 1] = red;

            map.ParallaxScrollingVector = new Vector(4.0f, 0.5f);
            map.Offset = new Point(-25, 0);

            return map;
        }

        private TileMap CreateTileMap()
        {
            var texture = this.gameResourceManager.GetTexture(@"Sandbox\TileSheet");
            var sheet = new TileSheet(texture, "Background", new Size(16, 16));
            var red = sheet.CreateTileDefinition("red", new Point(0, 0));
            var green = sheet.CreateTileDefinition("green", new Point(16, 0));
            sheet.CreateTileDefinition("yellow", new Point(32, 0));
            var purple = sheet.CreateTileDefinition("purple", new Point(0, 16));
            sheet.CreateTileDefinition("orange", new Point(16, 16));
            sheet.CreateTileDefinition("blue", new Point(32, 16));

            //sheet.Save(@"C:\Users\Pascal\Dev\DotNet\GitHub\XNAGameEngine2D\Color TileSheet.xml");

            this.gameResourceManager.AddTileSheet(sheet);

            var tileMap = new TileMap("Tiles", new Size(32, 32), new Size(16, 16));
            tileMap[0, 0] = purple;
            tileMap[1, 1] = red;
            tileMap[10, 10] = purple;
            tileMap[4, 20] = green;

            tileMap.ParallaxScrollingVector = new Vector(2.0f, 2.0f);
            tileMap.Offset = new Point(0, -20);

            return tileMap;
        }

        private SpriteMap CreateSpriteMap()
        {
            var texture = this.gameResourceManager.GetTexture(@"Sandbox\LinkSheet");
            var sheet = new SpriteSheet(texture, "Link");
            sheet.CreateSpriteDefinition("Link01", new Rectangle(3, 3, 16, 22));
            sheet.CreateSpriteDefinition("Sleep01", new Rectangle(45, 219, 32, 40));

            //sheet.Save(@"C:\Users\Pascal\Dev\DotNet\GitHub\XNAGameEngine2D\Link SpriteSheet.xml");

            var link01 = new Sprite(sheet, "Link01") { Position = this.player };
            var sleep01 = new Sprite(sheet, "Sleep01") { Position = new Point(125, 25) };

            this.gameResourceManager.AddSpriteSheet(sheet);

            var spriteMap = new SpriteMap("Sprites");
            spriteMap.AddSprite(link01);
            spriteMap.AddSprite(sleep01);

            spriteMap.ParallaxScrollingVector = new Vector(4.0f, 8.0f);
            spriteMap.Offset = new Point(50, 50);

            return spriteMap;
        }

        private DrawingMap CreateHexMapTestDistance()
        {
            var map = new DrawingMap("Hex drawing test", this.gameResourceManager);
            var font = this.gameResourceManager.GetDrawingFont(@"Sandbox\SpriteFont1");
            var hexMap = HexGrid.CreateHexMap(30, 9);
            foreach (var hex in hexMap.Hexes)
            {
                var distance = HexGrid.HexDistance(hexMap[4, 5], hex);

                var color = distance == 1
                    ? new Color(0, 255, 0, 255)
                    : distance == 2
                        ? new Color(0, 192, 0, 255)
                        : distance == 3
                            ? new Color(0, 128, 0, 255)
                            : distance == 4
                                ? new Color(0, 64, 0, 255)
                                : distance == 5
                                    ? new Color(128, 0, 128, 255)
                                    : Color.Red;

                map.AddPolygone(3, color, hex.GetVertices());

                var text = string.Format("{0},{1}", hex.Position.X - 4, hex.Position.Y - 5 + (hex.Position.X % 2) * .5);
                var measure = font.MeasureString(text);

                map.AddText(font, text, hex.Center - (measure / 2.0f), Color.Yellow);
            }

            return map;
        }
    }
}
