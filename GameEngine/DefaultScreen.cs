using System;
using System.Linq;

using ClassLibrary;
using ClassLibrary.Cameras;
using ClassLibrary.Drawing;
using ClassLibrary.Hexes;
using ClassLibrary.Inputs;
using ClassLibrary.Maps;
using ClassLibrary.Scenes;
using ClassLibrary.Sprites;
using ClassLibrary.Tiles;

using Microsoft.Xna.Framework;

using WindowsGame1.EngineImplementation;

using Point = ClassLibrary.Point;

namespace WindowsGame1
{
    public class DefaultScreen
    {
        private Point player;

        private float range;

        private Scene scene;

        private GameResourceManager gameResourceManager;

        private InputConfiguration inputConfiguration;

        private TextElement fpsElement;

        private TextElement viewPortElement;

        private TextElement translationElement;

        private TextElement positionElement;

        private TextElement zoomingElement;

        private TextElement rangeElement;

        public void Initialize()
        {
            this.player = new ClassLibrary.Point(25, 25);
            this.range = 0.25f;
        }

        public void LoadContent(Camera camera, GameResourceManager resourceManager)
        {
            this.gameResourceManager = resourceManager;

            this.InitInput(camera);

            this.gameResourceManager.AddDrawingFont("SpriteFont1");

            this.gameResourceManager.AddTexture("LinkSheet");
            this.gameResourceManager.AddTexture("HexSheet");
            this.gameResourceManager.AddTexture("TileSheet");

            this.CreateScene();

            var scene2 = Scene.LoadFrom(this.gameResourceManager,
                @"C:\Users\Pascal\Dev\DotNet\GitHub\XNAGameEngine2D\TestScene.xml");

            foreach (var map in this.scene.Maps)
            {
                var otherMap = scene2.Maps.Single(x => x.Name == map.Name);

                var myXml = map.ToXml();
                var otherXml = otherMap.ToXml();

                if (myXml.ToString() != otherXml.ToString())
                {
                    //Console.WriteLine(myXml.ToString());
                    //Console.WriteLine(otherXml.ToString());
                }
            }
        }

        public void Update(GameTime gameTime)
        {
            this.inputConfiguration.Update(new XnaInputContext(), gameTime.ElapsedGameTime.TotalSeconds);

            var colorMap = this.scene.Maps.OfType<ColorMap>().FirstOrDefault();
            if (colorMap != null)
                colorMap.Color = new ClassLibrary.Color(255, 0, 0, (int)(255 * Math.Min(this.range, 1.0f)));
        }

        public void Draw(long fps, Camera camera, DrawContext drawContext)
        {
            this.fpsElement.SetParameters(fps);
            this.viewPortElement.SetParameters(camera.SceneViewPort);
            this.translationElement.SetParameters(camera.SceneTranslationVector);
            this.positionElement.SetParameters(camera.Position);
            this.zoomingElement.SetParameters(camera.ZoomFactor);
            this.rangeElement.SetParameters(this.range);

            this.scene.Draw(drawContext, camera);
        }

        private void InitInput(Camera camera)
        {
            this.inputConfiguration = new InputConfiguration();
            this.inputConfiguration.AddDigitalButton("Left").Assign(KeyboardKeys.Left).MapTo(elapse => camera.Move(-60 * elapse, 0));
            this.inputConfiguration.AddDigitalButton("Right").Assign(KeyboardKeys.Right).MapTo(elapse => camera.Move(60 * elapse, 0));
            this.inputConfiguration.AddDigitalButton("Up").Assign(KeyboardKeys.Up).MapTo(elapse => camera.Move(0, -60 * elapse));
            this.inputConfiguration.AddDigitalButton("Down").Assign(KeyboardKeys.Down).MapTo(elapse => camera.Move(0, 60 * elapse));
            this.inputConfiguration.AddDigitalButton("ZoomIn").Assign(KeyboardKeys.A).MapTo(elapse => camera.ZoomFactor *= 1.2f * (1 + elapse));
            this.inputConfiguration.AddDigitalButton("ZoomOut").Assign(KeyboardKeys.Z).MapTo(elapse => camera.ZoomFactor *= 1 / (1.2f * (1 + elapse)));
            this.inputConfiguration.AddDigitalButton("RangeUp").Assign(KeyboardKeys.W).MapTo(elapse => this.range *= 1.2f * (1 + elapse));
            this.inputConfiguration.AddDigitalButton("RangeDown").Assign(KeyboardKeys.Q).MapTo(elapse => this.range *= 1 / (1.2f * (1 + elapse)));
        }

        private void CreateScene()
        {
            this.scene = new Scene("scene1");

            this.scene.AddMap(this.CreateImageMap());
            this.scene.AddMap(this.CreateHexTest());
            this.scene.AddMap(this.CreateTileTest());
            this.scene.AddMap(this.CreateColorMap());
            this.scene.AddMap(this.CreateHexMapTestDistance());
            this.scene.AddMap(this.CreateSpriteTest());
            this.scene.AddMap(this.CreateDiagnosticText());

            this.scene.Save(@"C:\Users\Pascal\Dev\DotNet\GitHub\XNAGameEngine2D\TestScene.xml");
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

            this.gameResourceManager.AddHexSheet(sheet);

            var map = new HexMap("Hex", new Size(4, 4), new Size(60, 52));
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