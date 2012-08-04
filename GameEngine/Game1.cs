using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using WindowsGame1.Cameras;
using WindowsGame1.Hexes;
using WindowsGame1.Maps;
using WindowsGame1.Scenes;
using WindowsGame1.Sprites;
using WindowsGame1.Tiles;

namespace WindowsGame1
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont courierNew;
        private Texture2D linkSheet;
        private Point player;
        private Camera camera;
        private float elapseTime;
        private long frameCounter;
        private long fps;
        private Texture2D tileSheet;
        private Texture2D hexSheet;
        private float range = 1.0f;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
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
            this.player = new Point(25, 25);
            this.range = 0.25f;

            this.camera = new Camera(this.graphics.GraphicsDevice.Viewport);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            courierNew = Content.Load<SpriteFont>("SpriteFont1");

            this.linkSheet = Content.Load<Texture2D>("LinkSheet");
            this.tileSheet = Content.Load<Texture2D>("TileSheet");
            this.hexSheet = Content.Load<Texture2D>("HexSheet");
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

            if (elapseTime > 1)
            {
                this.fps = frameCounter;
                frameCounter = 0;
                elapseTime = 0;
            }

            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here
            var keyState = Keyboard.GetState();

            //if (keyState.IsKeyDown(Keys.Left))
            //    this.player = this.player.Translate(new Point(-1, 0));
            //if (keyState.IsKeyDown(Keys.Right))
            //    this.player = this.player.Translate(new Point(1, 0));
            //if (keyState.IsKeyDown(Keys.Up))
            //    this.player = this.player.Translate(new Point(0, -1));
            //if (keyState.IsKeyDown(Keys.Down))
            //    this.player = this.player.Translate(new Point(0, 1));

            //if (keyState.IsKeyDown(Keys.A)) this.range *= 1.02f;
            //if (keyState.IsKeyDown(Keys.Z)) this.range *= 1 / 1.02f;
            if (keyState.IsKeyDown(Keys.Left)) this.camera.Move(-1, 0);
            if (keyState.IsKeyDown(Keys.Right)) this.camera.Move(1, 0);
            if (keyState.IsKeyDown(Keys.Up)) this.camera.Move(0, -1);
            if (keyState.IsKeyDown(Keys.Down)) this.camera.Move(0, 1);

            if (keyState.IsKeyDown(Keys.A)) this.camera.ZoomFactor *= 1.02f;
            if (keyState.IsKeyDown(Keys.Z)) this.camera.ZoomFactor *= 1 / 1.02f;
            if (keyState.IsKeyDown(Keys.W)) this.range *= 1.02f;
            if (keyState.IsKeyDown(Keys.Q)) this.range *= 1 / 1.02f;

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();

            Texture2D blank = new Texture2D(GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            blank.SetData(new[] { Color.White });

            // FPS
            this.spriteBatch.DrawString(this.courierNew, "FPS " + this.fps.ToString("d"), new Vector2(610, 0), Color.White);
            this.spriteBatch.DrawString(this.courierNew, string.Format("ViewPort: {0}", this.camera.SceneViewPort), new Vector2(410, 20), Color.White);
            this.spriteBatch.DrawString(this.courierNew, string.Format("Translation: {0}", this.camera.SceneTranslationVector), new Vector2(410, 40), Color.White);
            this.spriteBatch.DrawString(this.courierNew, string.Format("Position: {0}", this.camera.Position), new Vector2(410, 60), Color.White);
            this.spriteBatch.DrawString(this.courierNew, string.Format("Zooming: {0:f1}", this.camera.ZoomFactor), new Vector2(410, 80), Color.White);

            var scene = new Scene();

            scene.AddMap(this.DrawImageMap());
            scene.AddMap(this.DrawHexTest());
            scene.AddMap(this.DrawTileTest());
            scene.AddMap(this.DrawColorMap());
            scene.AddMap(this.DrawSpriteTest());
            scene.Draw(this.spriteBatch, this.camera);


            this.DrawHexMapTestDistance(blank);
            this.DrawCamera(blank);

            this.spriteBatch.End();

            base.Draw(gameTime);
        }

        private ImageMap DrawImageMap()
        {
            //var map = ImageMap.CreateFillScreenImageMap(this.GraphicsDevice, this.linkSheet);
            var map = new ImageMap(this.linkSheet, new Rectangle(10, 10, 250, 250));
            //map.Draw(this.spriteBatch, this.camera);

            return map;
        }

        private ColorMap DrawColorMap()
        {
            var alpha = Math.Min(this.range, 1.0f);
            var map = new ColorMap(this.GraphicsDevice, Color.Red * alpha);
            //map.Draw(this.spriteBatch, this.camera);
            
            return map;
        }

        private void DrawCamera(Texture2D blank)
        {
            this.DrawLine(this.spriteBatch, blank, 1.0f, Color.Yellow,
                this.camera.ViewPortCenter.Translate(-10, 0).ToVector(),
                this.camera.ViewPortCenter.Translate(10, 0).ToVector());

            this.DrawLine(this.spriteBatch, blank, 1.0f, Color.Yellow,
                this.camera.ViewPortCenter.Translate(0, -10).ToVector(),
                this.camera.ViewPortCenter.Translate(0, 10).ToVector());
        }

        private HexMap DrawHexTest()
        {
            var sheet = new HexSheet(this.hexSheet, "Hexes", new Size(68, 60));
            var red = sheet.CreateTileDefinition("red", new Point(55, 30));
            var yellow = sheet.CreateTileDefinition("yellow", new Point(163, 330));
            var purple = sheet.CreateTileDefinition("purple", new Point(488, 330));

            var map = new HexMap(new Size(4, 4), new Size(60, 52));
            map[2, 0] = purple;
            map[2, 1] = purple;
            map[2, 2] = yellow;
            map[0, 1] = red;
            map[1, 1] = red;

            map.ParallaxScrollingVector = new Vector2(4.0f, 0.5f);
            //map.Draw(this.spriteBatch, this.camera);

            return map;
        }

        private TileMap DrawTileTest()
        {
            var sheet = new TileSheet(this.tileSheet, "Background", new Size(16, 16));
            var red = sheet.CreateTileDefinition("red", new Point(0, 0));
            var green = sheet.CreateTileDefinition("green", new Point(16, 0));
            sheet.CreateTileDefinition("yellow", new Point(32, 0));
            var purple = sheet.CreateTileDefinition("purple", new Point(0, 16));
            sheet.CreateTileDefinition("orange", new Point(16, 16));
            sheet.CreateTileDefinition("blue", new Point(32, 16));

            var tileMap = new TileMap(new Size(32, 32), new Size(16, 16));
            tileMap[0, 0] = purple;
            tileMap[1, 1] = red;
            tileMap[10, 10] = purple;
            tileMap[4, 20] = green;

            tileMap.ParallaxScrollingVector = new Vector2(2.0f, 2.0f);
            //tileMap.Draw(this.spriteBatch, this.camera);

            return tileMap;
        }

        private SpriteMap DrawSpriteTest()
        {
            var sheet = new SpriteSheet(this.linkSheet, "Link");
            sheet.CreateSpriteDefinition("Link01", new Rectangle(3, 3, 16, 22));
            sheet.CreateSpriteDefinition("Sleep01", new Rectangle(45, 219, 32, 40));

            var link01 = new Sprite(sheet, "Link01") { Position = this.player };
            var sleep01 = new Sprite(sheet, "Sleep01") { Position = new Point(125, 25) };

            var spriteMap = new SpriteMap();
            spriteMap.AddSprite(link01);
            spriteMap.AddSprite(sleep01);

            spriteMap.ParallaxScrollingVector = new Vector2(4.0f, 8.0f);
            //spriteMap.Draw(this.spriteBatch, this.camera);

            return spriteMap;
        }

        private void DrawHexMapTestDistance(Texture2D blank)
        {
            var hexMap = HexGrid.CreateHexMap(30, 9);
            foreach (var hex in hexMap.Hexes) //this.HexCenters(20, 650))
            {
                var distance = HexGrid.HexDistance(hexMap[4, 5], hex);

                var color = distance == 1
                    ? Color.FromNonPremultiplied(0, 255, 0, 255)
                    : distance == 2
                        ? Color.FromNonPremultiplied(0, 192, 0, 255)
                        : distance == 3
                            ? Color.FromNonPremultiplied(0, 128, 0, 255)
                            : distance == 4
                                ? Color.FromNonPremultiplied(0, 64, 0, 255)
                                : distance == 5
                                    ? Color.FromNonPremultiplied(128, 0, 128, 255)
                                    : Color.Red;

                //this.DrawLine(this.spriteBatch, blank, 1.0f, Color.White, hex.Center, hex.Center + new Vector2(0, 1));
                //this.DrawLine(this.spriteBatch, blank, 1.0f, Color.White, hex.Center, hex.Center + new Vector2(1, 0));
                this.DrawHex(this.spriteBatch, this.camera, blank, color, hex);

                //var text = string.Format("{0},{1}", hex.Position.X, hex.Position.Y);
                //var text = string.Format("{0},{1}", hex.Position.X - 4, (hex.Position.Y - 4) * 2 + hex.Position.X % 2);
                //var text = string.Format("{0},{1}", hex.Position.X - 4, hex.Position.Y - 4);
                var text = string.Format("{0},{1}", hex.Position.X - 4, hex.Position.Y - 5 + (hex.Position.X % 2) * .5);
                //var text = string.Format("{0},{1}", hex.Position.X - 4, hex.Position.Y - 4 + (hex.Position.X % 2) * .5);
                //var text = string.Format("{0},{1}", hex.Position.X - 5, hex.Position.Y - 5 - ((hex.Position.X + 1) % 2) * .5);
                //var text = string.Format("{0}", HexGrid.HexDistance(hexMap[3,3], hex));
                var measure = this.courierNew.MeasureString(text);

                this.spriteBatch.DrawString(this.courierNew, text,
                    (hex.Center - (measure / 2.0f))
                        .Scale(this.camera.ZoomFactor)
                        .Translate(this.camera.GetSceneTranslationVector(new Vector2(0.5f, 2.0f))),
                    Color.Yellow, 0.0f, Vector2.Zero, this.camera.ZoomFactor, SpriteEffects.None, 0.0f);
            }
        }

        void DrawLine(SpriteBatch batch, Texture2D blank,
                      float width, Color color, Vector2 point1, Vector2 point2)
        {
            float angle = (float)Math.Atan2(point2.Y - point1.Y, point2.X - point1.X);
            float length = Vector2.Distance(point1, point2);

            batch.Draw(blank, point1, null, color,
                       angle, Vector2.Zero, new Vector2(length, width),
                       SpriteEffects.None, 0);
        }

        //private void DrawHex(SpriteBatch batch, Texture2D blank, Color color, Vector2 center, float size)
        //{
        //    DrawHex(batch, blank, color, new Hex(center, size));
        //}

        private void DrawHex(SpriteBatch batch, Camera camera1, Texture2D blank, Color color, HexGridElement hex)
        {
            var vertices = new List<Vector2>(hex.GetVertices()
                .Select(v => v.Scale(camera1.ZoomFactor).Translate(camera1.GetSceneTranslationVector(new Vector2(0.5f, 2.0f)))));

            vertices.Add(vertices.First());

            for (var i = 0; i < 6; i++)
            {
                this.DrawLine(batch, blank, 3, color, vertices[i], vertices[i + 1]);
            }
        }
    }
}
