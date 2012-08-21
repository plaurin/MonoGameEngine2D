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
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Color = Microsoft.Xna.Framework.Color;
using Texture = ClassLibrary.Texture;

namespace WindowsGame1
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {
        private readonly GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        //private SpriteFont courierNew;
        //private Texture2D linkSheet;
        private ClassLibrary.Point player;
        private Camera camera;
        private float elapseTime;
        private long frameCounter;
        private long fps;
        //private Texture2D tileSheet;
        //private Texture2D hexSheet;
        private float range = 1.0f;

        private Scene scene;

        //private Texture2D whitePixel;

        private GameResourceManager gameResourceManager;

        private InputConfiguration inputConfiguration;

        private TextElement fpsElement;

        private TextElement viewPortElement;

        private TextElement translationElement;

        private TextElement positionElement;

        private TextElement zoomingElement;

        private TextElement rangeElement;

        public Game1()
        {
            this.graphics = new GraphicsDeviceManager(this);
            this.Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            this.player = new ClassLibrary.Point(25, 25);
            this.range = 0.25f;

            this.camera = XnaCamera.CreateCamera(this.graphics.GraphicsDevice.Viewport);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            this.spriteBatch = new SpriteBatch(this.GraphicsDevice);
            this.gameResourceManager = new XnaGameResourceManager(this.Content);

            // TODO: use this.Content to load your game content here
            this.InitInput();

            //this.courierNew = this.Content.Load<SpriteFont>("SpriteFont1");
            this.gameResourceManager.AddDrawingFont("SpriteFont1");

            this.gameResourceManager.AddTexture("WhitePixel", CreateTexture(this.GraphicsDevice));

            this.gameResourceManager.AddTexture("LinkSheet");
            this.gameResourceManager.AddTexture("HexSheet");
            this.gameResourceManager.AddTexture("TileSheet");

            //this.linkSheet = this.Content.Load<Texture2D>("LinkSheet");
            //this.linkSheet.Name = "LinkSheet";

            //this.tileSheet = this.Content.Load<Texture2D>("TileSheet");
            //this.hexSheet = this.Content.Load<Texture2D>("HexSheet");
            //this.whitePixel = CreateTexture(this.GraphicsDevice);

            this.CreateScene();
            //this.scene = new Scene();

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

        private void InitInput()
        {
            this.inputConfiguration = new InputConfiguration();
            this.inputConfiguration.AddDigitalButton("Left").Assign(KeyboardKeys.Left).MapTo(elapse => this.camera.Move(-60 * elapse, 0));
            this.inputConfiguration.AddDigitalButton("Right").Assign(KeyboardKeys.Right).MapTo(elapse => this.camera.Move(60 * elapse, 0));
            this.inputConfiguration.AddDigitalButton("Up").Assign(KeyboardKeys.Up).MapTo(elapse => this.camera.Move(0, -60 * elapse));
            this.inputConfiguration.AddDigitalButton("Down").Assign(KeyboardKeys.Down).MapTo(elapse => this.camera.Move(0, 60 * elapse));
            this.inputConfiguration.AddDigitalButton("ZoomIn").Assign(KeyboardKeys.A).MapTo(elapse => this.camera.ZoomFactor *= 1.2f * (1 + elapse));
            this.inputConfiguration.AddDigitalButton("ZoomOut").Assign(KeyboardKeys.Z).MapTo(elapse => this.camera.ZoomFactor *= 1 / (1.2f * (1 + elapse)));
            this.inputConfiguration.AddDigitalButton("RangeUp").Assign(KeyboardKeys.W).MapTo(elapse => this.range *= 1.2f * (1 + elapse));
            this.inputConfiguration.AddDigitalButton("RangeDown").Assign(KeyboardKeys.Q).MapTo(elapse => this.range *= 1 / (1.2f * (1 + elapse)));
            //this.inputConfiguration.AddDigitalButton("Left").Assign(KeyboardKeys.Left).MapTo(() => this.camera.Move(-1, 0));
            //this.inputConfiguration.AddDigitalButton("Right").Assign(KeyboardKeys.Right).MapTo(() => this.camera.Move(1, 0));
            //this.inputConfiguration.AddDigitalButton("Up").Assign(KeyboardKeys.Up).MapTo(() => this.camera.Move(0, -1));
            //this.inputConfiguration.AddDigitalButton("Down").Assign(KeyboardKeys.Down).MapTo(() => this.camera.Move(0, 1));
            //this.inputConfiguration.AddDigitalButton("ZoomIn").Assign(KeyboardKeys.A).MapTo(() => this.camera.ZoomFactor *= 1.02f);
            //this.inputConfiguration.AddDigitalButton("ZoomOut").Assign(KeyboardKeys.Z).MapTo(() => this.camera.ZoomFactor *= 1 / 1.02f);
            //this.inputConfiguration.AddDigitalButton("RangeUp").Assign(KeyboardKeys.W).MapTo(() => this.range *= 1.02f);
            //this.inputConfiguration.AddDigitalButton("RangeDown").Assign(KeyboardKeys.Q).MapTo(() => this.range *= 1 / 1.02f);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // FPS
            this.elapseTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            this.frameCounter++;

            if (this.elapseTime > 1)
            {
                this.fps = this.frameCounter;
                this.frameCounter = 0;
                this.elapseTime = 0;
            }

            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here
            this.inputConfiguration.Update(new XnaInputContext(), gameTime.ElapsedGameTime.TotalSeconds);

            var colorMap = this.scene.Maps.OfType<ColorMap>().FirstOrDefault();
            if (colorMap != null)
                colorMap.Color = new ClassLibrary.Color(255, 0, 0, (int)(255 * Math.Min(this.range, 1.0f)));

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            this.GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            this.spriteBatch.Begin();

            Texture2D blank = new Texture2D(this.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            blank.SetData(new[] { Color.White });

            // FPS
            //this.spriteBatch.DrawString(this.courierNew, "FPS " + this.fps.ToString("d"), new Vector2(610, 0), Color.White);
            //this.spriteBatch.DrawString(this.courierNew, String.Format("ViewPort: {0}", this.camera.SceneViewPort), new Vector2(410, 20), Color.White);
            //this.spriteBatch.DrawString(this.courierNew, String.Format("Translation: {0}", this.camera.SceneTranslationVector), new Vector2(410, 40), Color.White);
            //this.spriteBatch.DrawString(this.courierNew, String.Format("Position: {0}", this.camera.Position), new Vector2(410, 60), Color.White);
            //this.spriteBatch.DrawString(this.courierNew, String.Format("Zooming: {0:f1}", this.camera.ZoomFactor), new Vector2(410, 80), Color.White);
            //this.spriteBatch.DrawString(this.courierNew, String.Format("Range: {0:f1}", this.range), new Vector2(410, 100), Color.White);
            this.fpsElement.SetParameters(this.fps);
            this.viewPortElement.SetParameters(this.camera.SceneViewPort);
            this.translationElement.SetParameters(this.camera.SceneTranslationVector);
            this.positionElement.SetParameters(this.camera.Position);
            this.zoomingElement.SetParameters(this.camera.ZoomFactor);
            this.rangeElement.SetParameters(this.range);

            var drawContext = new XnaDrawContext(this.spriteBatch, blank, this.graphics.GraphicsDevice.Viewport);
            this.scene.Draw(drawContext, this.camera);

            //this.DrawHexMapTestDistance(blank);
            this.DrawCamera(drawContext);

            this.spriteBatch.End();

            base.Draw(gameTime);
        }

        private Scene CreateScene()
        {
            this.scene = new Scene("scene1");

            this.scene.AddMap(this.DrawImageMap());
            this.scene.AddMap(this.DrawHexTest());
            this.scene.AddMap(this.DrawTileTest());
            this.scene.AddMap(this.DrawColorMap());
            this.scene.AddMap(this.DrawHexMapTestDistance(null));
            this.scene.AddMap(this.DrawSpriteTest());
            this.scene.AddMap(this.DrawText());

            this.scene.Save(@"C:\Users\Pascal\Dev\DotNet\GitHub\XNAGameEngine2D\TestScene.xml");

            return this.scene;
        }

        private MapBase DrawText()
        {
            //this.spriteBatch.DrawString(this.courierNew, "FPS " + this.fps.ToString("d"), new Vector2(610, 0), Color.White);
            //this.spriteBatch.DrawString(this.courierNew, String.Format("ViewPort: {0}", this.camera.SceneViewPort), new Vector2(410, 20), Color.White);
            //this.spriteBatch.DrawString(this.courierNew, String.Format("Translation: {0}", this.camera.SceneTranslationVector), new Vector2(410, 40), Color.White);
            //this.spriteBatch.DrawString(this.courierNew, String.Format("Position: {0}", this.camera.Position), new Vector2(410, 60), Color.White);
            //this.spriteBatch.DrawString(this.courierNew, String.Format("Zooming: {0:f1}", this.camera.ZoomFactor), new Vector2(410, 80), Color.White);
            //this.spriteBatch.DrawString(this.courierNew, String.Format("Range: {0:f1}", this.range), new Vector2(410, 100), Color.White);
            var map = new DrawingMap("Diagnostics", this.gameResourceManager);
            map.CameraMode = CameraMode.Fix;

            var font = this.gameResourceManager.GetDrawingFont("SpriteFont1");
            this.fpsElement = map.AddText(font, "FPS {0:d}", new Vector(610, 0), ClassLibrary.Color.White);
            this.viewPortElement = map.AddText(font, "ViewPort: {0}", new Vector(410, 20), ClassLibrary.Color.White);
            this.translationElement = map.AddText(font, "Translation: {0}", new Vector(410, 40), ClassLibrary.Color.White);
            this.positionElement = map.AddText(font, "Position: {0}", new Vector(410, 60), ClassLibrary.Color.White);
            this.zoomingElement = map.AddText(font, "Zooming: {0:f1}", new Vector(410, 80), ClassLibrary.Color.White);
            this.rangeElement = map.AddText(font, "Range: {0:f1}", new Vector(410, 100), ClassLibrary.Color.White);
            return map;
        }

        private ImageMap DrawImageMap()
        {
            //var map = ImageMap.CreateFillScreenImageMap(this.GraphicsDevice, this.linkSheet);
            var texture = this.gameResourceManager.GetTexture("LinkSheet");
            var map = new ImageMap("Image", texture, new ClassLibrary.Rectangle(10, 10, 250, 250));
            //map.Draw(this.spriteBatch, this.camera);

            return map;
        }

        private ColorMap DrawColorMap()
        {
            var alpha = Math.Min(this.range, 1.0f);
            var map = new ColorMap("Red", new ClassLibrary.Color(255, 0, 0, (int)(255 * alpha)));
            //map.Draw(this.spriteBatch, this.camera);
            
            return map;
        }

        private void DrawCamera(DrawContext drawContext)
        {
            drawContext.DrawLine(
                this.camera.ViewPortCenter.Translate(-10, 0).ToVector(),
                this.camera.ViewPortCenter.Translate(10, 0).ToVector(), 1.0f, new ClassLibrary.Color(255, 255, 0, 255));

            drawContext.DrawLine(
                this.camera.ViewPortCenter.Translate(0, -10).ToVector(),
                this.camera.ViewPortCenter.Translate(0, 10).ToVector(), 1.0f, new ClassLibrary.Color(255, 255, 0, 255));
            //this.DrawLine(this.spriteBatch, blank, 1.0f, Color.Yellow,
            //    this.camera.ViewPortCenter.Translate(-10, 0).ToVector(),
            //    this.camera.ViewPortCenter.Translate(10, 0).ToVector());

            //this.DrawLine(this.spriteBatch, blank, 1.0f, Color.Yellow,
            //    this.camera.ViewPortCenter.Translate(0, -10).ToVector(),
            //    this.camera.ViewPortCenter.Translate(0, 10).ToVector());
        }

        private HexMap DrawHexTest()
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
            //map.Draw(this.spriteBatch, this.camera);

            return map;
        }

        private TileMap DrawTileTest()
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
            //tileMap.Draw(this.spriteBatch, this.camera);

            return tileMap;
        }

        private SpriteMap DrawSpriteTest()
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
            //spriteMap.Draw(this.spriteBatch, this.camera);

            return spriteMap;
        }

        private DrawingMap DrawHexMapTestDistance(Texture2D blank)
        {
            var map = new DrawingMap("Hex drawing test", this.gameResourceManager);
            var font = this.gameResourceManager.GetDrawingFont("SpriteFont1");
            //var font = this.Content.Load<SpriteFont>("SpriteFont1");
            var hexMap = HexGrid.CreateHexMap(30, 9);
            foreach (var hex in hexMap.Hexes) //this.HexCenters(20, 650))
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

                //this.DrawLine(this.spriteBatch, blank, 1.0f, Color.White, hex.Center, hex.Center + new Vector2(0, 1));
                //this.DrawLine(this.spriteBatch, blank, 1.0f, Color.White, hex.Center, hex.Center + new Vector2(1, 0));
                
                this.DrawHex(color, hex, map);

                //var text = string.Format("{0},{1}", hex.Position.X, hex.Position.Y);
                //var text = string.Format("{0},{1}", hex.Position.X - 4, (hex.Position.Y - 4) * 2 + hex.Position.X % 2);
                //var text = string.Format("{0},{1}", hex.Position.X - 4, hex.Position.Y - 4);
                var text = string.Format("{0},{1}", hex.Position.X - 4, hex.Position.Y - 5 + (hex.Position.X % 2) * .5);
                //var text = string.Format("{0},{1}", hex.Position.X - 4, hex.Position.Y - 4 + (hex.Position.X % 2) * .5);
                //var text = string.Format("{0},{1}", hex.Position.X - 5, hex.Position.Y - 5 - ((hex.Position.X + 1) % 2) * .5);
                //var text = string.Format("{0}", HexGrid.HexDistance(hexMap[3,3], hex));
                
                var measure = font.MeasureString(text);

                //this.spriteBatch.DrawString(font.Font, text,
                //    (hex.Center - (measure / 2.0f))
                //        .Scale(this.camera.ZoomFactor)
                //        .Translate(this.camera.GetSceneTranslationVector(new Vector2(0.5f, 2.0f))),
                //    Color.Yellow, 0.0f, Vector2.Zero, this.camera.ZoomFactor, SpriteEffects.None, 0.0f);
                map.AddText(font, text, hex.Center - (measure / 2.0f), ClassLibrary.Color.Yellow);
            }

            return map;
        }

        private void DrawLine(SpriteBatch batch, Texture2D blank, float width, Color color, Vector2 point1, Vector2 point2)
        {
            float angle = (float)Math.Atan2(point2.Y - point1.Y, point2.X - point1.X);
            float length = Vector2.Distance(point1, point2);

            batch.Draw(blank, point1, null, color,
                       angle, Vector2.Zero, new Vector2(length, width),
                       SpriteEffects.None, 0);
        }

        private void DrawHex(ClassLibrary.Color color, HexGridElement hex, DrawingMap map)
        {
            map.AddPolygone(3, color, hex.GetVertices());
        }

        private static Texture CreateTexture(GraphicsDevice device)
        {
            var rectangleTexture = new Texture2D(device, 1, 1, false, SurfaceFormat.Color)
            {
                Name = "WhitePixel"
            };

            rectangleTexture.SetData(new[] { Color.White });
            return new XnaTexture(rectangleTexture);
        }
    }

    //public enum CameraMode
    //{
    //    Follow = 0,
    //    Fix = 1
    //}
}
