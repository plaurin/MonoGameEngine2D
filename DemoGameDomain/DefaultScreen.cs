using System;
using System.Diagnostics;
using System.Linq;

using ClassLibrary;
using ClassLibrary.Cameras;
using ClassLibrary.Drawing;
using ClassLibrary.Hexes;
using ClassLibrary.Inputs;
using ClassLibrary.Maps;
using ClassLibrary.Scenes;
using ClassLibrary.Screens;
using ClassLibrary.Sheets;
using ClassLibrary.Sprites;
using ClassLibrary.Tiles;

namespace DemoGameDomain
{
    public class DefaultScreen : ScreenBase
    {
        private Camera camera;

        private GameResourceManager gameResourceManager;

        private Point player;

        private float range;

        private Scene scene;

        private MouseStateBase mouseState;

        private InputConfiguration inputConfiguration;

        private TextElement fpsElement;

        private TextElement viewPortElement;

        private TextElement translationElement;

        private TextElement positionElement;

        private TextElement zoomingElement;

        private TextElement rangeElement;

        private TextElement mouseElement;
        private TextElement mouseElement2;

        private DrawingMap mouseMap;

        public override void Initialize(Camera theCamera)
        {
            this.camera = theCamera;
            this.player = new ClassLibrary.Point(25, 25);
            this.range = 0.25f;
        }

        public override InputConfiguration GetInputConfiguration()
        {
            this.inputConfiguration = new InputConfiguration();

            this.inputConfiguration.AddDigitalButton("Left").Assign(KeyboardKeys.Left).MapTo(elapse => this.camera.Move(-60 * elapse, 0));
            this.inputConfiguration.AddDigitalButton("Right").Assign(KeyboardKeys.Right).MapTo(elapse => this.camera.Move(60 * elapse, 0));
            this.inputConfiguration.AddDigitalButton("Up").Assign(KeyboardKeys.Up).MapTo(elapse => this.camera.Move(0, -60 * elapse));
            this.inputConfiguration.AddDigitalButton("Down").Assign(KeyboardKeys.Down).MapTo(elapse => this.camera.Move(0, 60 * elapse));
            
            this.inputConfiguration.AddDigitalButton("ZoomIn")
                .Assign(KeyboardKeys.A).Assign(MouseButtons.Left)
                .MapTo(elapse => this.camera.ZoomFactor *= 1.2f * (1 + elapse));
            this.inputConfiguration.AddDigitalButton("ZoomOut")
                .Assign(KeyboardKeys.Z).Assign(MouseButtons.Right)
                .MapTo(elapse => this.camera.ZoomFactor *= 1 / (1.2f * (1 + elapse)));

            this.inputConfiguration.AddDigitalButton("RangeUp").Assign(KeyboardKeys.W).MapTo(elapse => this.range *= 1.2f * (1 + elapse));
            this.inputConfiguration.AddDigitalButton("RangeDown").Assign(KeyboardKeys.Q).MapTo(elapse => this.range *= 1 / (1.2f * (1 + elapse)));

            //this.inputConfiguration.AddDigitalButton("Selection").Assign(MouseButtons.Left).MapTo(elapse => this.range *= 1 / (1.2f * (1 + elapse)));
            this.inputConfiguration.AddMouseTracking(this.camera).OnMove((mt, e) => this.mouseState = mt);

            return this.inputConfiguration;
        }

        public override void LoadContent(GameResourceManager resourceManager)
        {
            this.gameResourceManager = resourceManager;

            this.gameResourceManager.AddDrawingFont("SpriteFont1");
        }

        public override void Update(double elapsedSeconds, int fps)
        {
            var colorMap = this.scene.Maps.OfType<ColorMap>().FirstOrDefault();
            if (colorMap != null)
                colorMap.Color = new ClassLibrary.Color(255, 0, 0, (int)(255 * Math.Min(this.range, 1.0f)));

            this.fpsElement.SetParameters(fps);
            this.viewPortElement.SetParameters(this.camera.SceneViewPort);
            this.translationElement.SetParameters(this.camera.SceneTranslationVector);
            this.positionElement.SetParameters(this.camera.Position);
            this.zoomingElement.SetParameters(this.camera.ZoomFactor);
            this.rangeElement.SetParameters(this.range);
            this.mouseElement.SetParameters(this.mouseState);
            this.mouseElement2.SetParameters(this.mouseState.AbsolutePosition);

            this.mouseMap.ClearAll();
            this.mouseMap.AddLine(this.mouseState.AbsolutePosition.Translate(-10, 0).ToVector(),
                this.mouseState.AbsolutePosition.Translate(10, 0).ToVector(), 2, Color.Red);

            this.mouseMap.AddLine(this.mouseState.AbsolutePosition.Translate(0, -10).ToVector(),
                this.mouseState.AbsolutePosition.Translate(0, 10).ToVector(), 2, Color.Red);
        }

        public override Scene GetScene()
        {
            this.scene = new Scene("scene1");

            this.scene.AddMap(this.CreateImageMap());
            this.scene.AddMap(this.CreateHexTest());
            this.scene.AddMap(this.CreateTileTest());
            this.scene.AddMap(this.CreateColorMap());
            this.scene.AddMap(this.CreateHexMapTestDistance());
            this.scene.AddMap(this.CreateSpriteTest());
            this.scene.AddMap(this.CreateDiagnosticText());
            this.scene.AddMap(this.CreateMouseCursorMap());

            this.scene.Save(@"C:\Users\Pascal\Dev\DotNet\GitHub\XNAGameEngine2D\TestScene.xml");

            this.TestSceneSaveLoad();
            this.TestSheetsSaveLoad();

            return this.scene;
        }

        private void TestSceneSaveLoad()
        {
            var scene2 = Scene.LoadFrom(this.gameResourceManager,
                @"C:\Users\Pascal\Dev\DotNet\GitHub\XNAGameEngine2D\TestScene.xml");

            foreach (var map in this.scene.Maps)
            {
                var otherMap = scene2.Maps.Single(x => x.Name == map.Name);

                var myXml = map.ToXml();
                var otherXml = otherMap.ToXml();

                if (myXml.ToString() != otherXml.ToString())
                {
                    //Debugger.Break();
                }
            }
        }

        private void TestSheetsSaveLoad()
        {
            var otherHexSheet = SheetBase.LoadFrom<HexSheet>(
                this.gameResourceManager, @"C:\Users\Pascal\Dev\DotNet\GitHub\XNAGameEngine2D\Color HexSheet.xml");

            var myHexSheetXml = this.gameResourceManager.GetHexSheet(otherHexSheet.Name).ToXml();
            var otherHexSheetXml = otherHexSheet.ToXml();
            if (myHexSheetXml.ToString() != otherHexSheetXml.ToString()) Debugger.Break();

            var otherTileSheet = SheetBase.LoadFrom<TileSheet>(
                this.gameResourceManager, @"C:\Users\Pascal\Dev\DotNet\GitHub\XNAGameEngine2D\Color TileSheet.xml");

            var myTileSheetXml = this.gameResourceManager.GetTileSheet(otherTileSheet.Name).ToXml();
            var otherTileSheetXml = otherTileSheet.ToXml();
            if (myTileSheetXml.ToString() != otherTileSheetXml.ToString()) Debugger.Break();

            var otherSpriteSheet = SheetBase.LoadFrom<SpriteSheet>(
                this.gameResourceManager, @"C:\Users\Pascal\Dev\DotNet\GitHub\XNAGameEngine2D\Link SpriteSheet.xml");

            var mySpriteSheetXml = this.gameResourceManager.GetSpriteSheet(otherSpriteSheet.Name).ToXml();
            var otherSpriteSheetXml = otherSpriteSheet.ToXml();
            if (mySpriteSheetXml.ToString() != otherSpriteSheetXml.ToString()) Debugger.Break();
        }

        private MapBase CreateMouseCursorMap()
        {
            this.mouseMap = new DrawingMap("MouseCusor", this.gameResourceManager) { CameraMode = CameraMode.Fix };

            return this.mouseMap;
        }

        private DrawingMap CreateDiagnosticText()
        {
            var map = new DrawingMap("Diagnostics", this.gameResourceManager) { CameraMode = CameraMode.Fix };

            var font = this.gameResourceManager.GetDrawingFont("SpriteFont1");
            this.fpsElement = map.AddText(font, "FPS {0:d}", new Vector(610, 0), ClassLibrary.Color.White);
            this.viewPortElement = map.AddText(font, "ViewPort: {0}", new Vector(410, 20), ClassLibrary.Color.White);
            this.translationElement = map.AddText(font, "Translation: {0}", new Vector(410, 40), ClassLibrary.Color.White);
            this.positionElement = map.AddText(font, "Position: {0}", new Vector(410, 60), ClassLibrary.Color.White);
            this.zoomingElement = map.AddText(font, "Zooming: {0:f1}", new Vector(410, 80), ClassLibrary.Color.White);
            this.rangeElement = map.AddText(font, "Range: {0:f1}", new Vector(410, 100), ClassLibrary.Color.White);
            this.mouseElement = map.AddText(font, "Mouse: {0}", new Vector(410, 120), ClassLibrary.Color.White);
            this.mouseElement2 = map.AddText(font, "MouseAbs: {0}", new Vector(410, 140), ClassLibrary.Color.White);

            return map;
        }

        private ImageMap CreateImageMap()
        {
            var texture = this.gameResourceManager.GetTexture("LinkSheet");

            return new ImageMap("Image", texture, new ClassLibrary.Rectangle(10, 10, 250, 250));
        }

        private ColorMap CreateColorMap()
        {
            var alpha = Math.Min(this.range, 1.0f);

            return new ColorMap("Red", new ClassLibrary.Color(255, 0, 0, (int)(255 * alpha)));
        }

        private HexMap CreateHexTest()
        {
            var texture = this.gameResourceManager.GetTexture("HexSheet");
            var sheet = new HexSheet(texture, "Hexes", new Size(68, 60));
            var red = sheet.CreateHexDefinition("red", new ClassLibrary.Point(55, 30));
            var yellow = sheet.CreateHexDefinition("yellow", new ClassLibrary.Point(163, 330));
            var purple = sheet.CreateHexDefinition("purple", new ClassLibrary.Point(488, 330));

            sheet.Save(@"C:\Users\Pascal\Dev\DotNet\GitHub\XNAGameEngine2D\Color HexSheet.xml");

            this.gameResourceManager.AddHexSheet(sheet);

            var map = new HexMap("Hex", new Size(4, 4), new Size(68, 60), 42);
            map[2, 0] = purple;
            map[2, 1] = purple;
            map[2, 2] = yellow;
            map[0, 1] = red;
            map[1, 1] = red;

            map.ParallaxScrollingVector = new Vector(4.0f, 0.5f);

            return map;
        }

        private TileMap CreateTileTest()
        {
            var texture = this.gameResourceManager.GetTexture("TileSheet");
            var sheet = new TileSheet(texture, "Background", new Size(16, 16));
            var red = sheet.CreateTileDefinition("red", new ClassLibrary.Point(0, 0));
            var green = sheet.CreateTileDefinition("green", new ClassLibrary.Point(16, 0));
            sheet.CreateTileDefinition("yellow", new ClassLibrary.Point(32, 0));
            var purple = sheet.CreateTileDefinition("purple", new ClassLibrary.Point(0, 16));
            sheet.CreateTileDefinition("orange", new ClassLibrary.Point(16, 16));
            sheet.CreateTileDefinition("blue", new ClassLibrary.Point(32, 16));

            sheet.Save(@"C:\Users\Pascal\Dev\DotNet\GitHub\XNAGameEngine2D\Color TileSheet.xml");

            this.gameResourceManager.AddTileSheet(sheet);

            var tileMap = new TileMap("Tiles", new Size(32, 32), new Size(16, 16));
            tileMap[0, 0] = purple;
            tileMap[1, 1] = red;
            tileMap[10, 10] = purple;
            tileMap[4, 20] = green;

            tileMap.ParallaxScrollingVector = new Vector(2.0f, 2.0f);

            return tileMap;
        }

        private SpriteMap CreateSpriteTest()
        {
            var texture = this.gameResourceManager.GetTexture("LinkSheet");
            var sheet = new SpriteSheet(texture, "Link");
            sheet.CreateSpriteDefinition("Link01", new ClassLibrary.Rectangle(3, 3, 16, 22));
            sheet.CreateSpriteDefinition("Sleep01", new ClassLibrary.Rectangle(45, 219, 32, 40));

            sheet.Save(@"C:\Users\Pascal\Dev\DotNet\GitHub\XNAGameEngine2D\Link SpriteSheet.xml");

            var link01 = new Sprite(sheet, "Link01") { Position = this.player };
            var sleep01 = new Sprite(sheet, "Sleep01") { Position = new ClassLibrary.Point(125, 25) };

            this.gameResourceManager.AddSpriteSheet(sheet);

            var spriteMap = new SpriteMap("Sprites");
            spriteMap.AddSprite(link01);
            spriteMap.AddSprite(sleep01);

            spriteMap.ParallaxScrollingVector = new Vector(4.0f, 8.0f);

            return spriteMap;
        }

        private DrawingMap CreateHexMapTestDistance()
        {
            var map = new DrawingMap("Hex drawing test", this.gameResourceManager);
            var font = this.gameResourceManager.GetDrawingFont("SpriteFont1");
            var hexMap = HexGrid.CreateHexMap(30, 9);
            foreach (var hex in hexMap.Hexes)
            {
                var distance = HexGrid.HexDistance(hexMap[4, 5], hex);

                var color = distance == 1
                    ? new ClassLibrary.Color(0, 255, 0, 255)
                    : distance == 2
                        ? new ClassLibrary.Color(0, 192, 0, 255)
                        : distance == 3
                            ? new ClassLibrary.Color(0, 128, 0, 255)
                            : distance == 4
                                ? new ClassLibrary.Color(0, 64, 0, 255)
                                : distance == 5
                                    ? new ClassLibrary.Color(128, 0, 128, 255)
                                    : ClassLibrary.Color.Red;

                map.AddPolygone(3, color, hex.GetVertices());

                var text = string.Format("{0},{1}", hex.Position.X - 4, hex.Position.Y - 5 + (hex.Position.X % 2) * .5);
                var measure = font.MeasureString(text);

                map.AddText(font, text, hex.Center - (measure / 2.0f), ClassLibrary.Color.Yellow);
            }

            return map;
        }
    }
}