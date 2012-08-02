using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using WindowsGame1.Hexes;
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

            if (keyState.IsKeyDown(Keys.Left))
                this.player = this.player.Translate(new Point(-1, 0));
            if (keyState.IsKeyDown(Keys.Right))
                this.player = this.player.Translate(new Point(1, 0));
            if (keyState.IsKeyDown(Keys.Up))
                this.player = this.player.Translate(new Point(0, -1));
            if (keyState.IsKeyDown(Keys.Down))
                this.player = this.player.Translate(new Point(0, 1));

            if (keyState.IsKeyDown(Keys.A)) this.range *= 1.02f;
            if (keyState.IsKeyDown(Keys.Z)) this.range *= 1 / 1.02f;

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
            spriteBatch.DrawString(courierNew, "FPS " + fps.ToString("d"), new Vector2(610, 0), Color.White);
            spriteBatch.DrawString(courierNew, "Range " + range.ToString("f2"), new Vector2(610, 20), Color.White);

            //this.DrawTileTest();
            this.DrawHexTest();
            //this.DrawHexMapTestDistance(blank);
            this.DrawSpriteTest();

            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void DrawHexTest()
        {
            var sheet = new HexSheet(this.hexSheet, "Hexes", new Size(68, 60));
            var red = sheet.CreateTileDefinition("red", new Point(55, 30));
            var yellow = sheet.CreateTileDefinition("yellow", new Point(163, 330));
            var purple = sheet.CreateTileDefinition("purple", new Point(488, 330));

            var map = new HexMap(new Size(4, 4), new Size(60, 52));
            map[2, 0] = purple;
            map[2, 1] = purple;
            map[2, 2] = purple;
            map[0, 1] = red;
            map[1, 1] = red;

            map.Scaling = this.range;
            map.Draw(this.spriteBatch);
        }

        private void DrawTileTest()
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

            tileMap.Scaling = this.range;
            tileMap.Draw(this.spriteBatch);
        }

        private void DrawSpriteTest()
        {
            var sheet = new SpriteSheet(this.linkSheet, "Link");
            sheet.CreateSpriteDefinition("Link01", new Rectangle(3, 3, 16, 22));
            sheet.CreateSpriteDefinition("Sleep01", new Rectangle(45, 219, 32, 40));

            var link01 = new Sprite(sheet, "Link01") { Position = this.player };
            var sleep01 = new Sprite(sheet, "Sleep01") { Position = new Point(125, 25) };

            var spriteMap = new SpriteMap();
            spriteMap.AddSprite(link01);
            spriteMap.AddSprite(sleep01);

            spriteMap.Scaling = this.range;

            spriteMap.Draw(this.spriteBatch);

            //link01.Draw(this.spriteBatch);
            //sleep01.Draw(this.spriteBatch);

            //this.spriteBatch.Draw(this.linkSheet, Vector2.Zero, Color.White);
            //this.spriteBatch.Draw(this.linkSheet, new Rectangle(10, 10, 16, 22), new Rectangle(3, 3, 16, 22), Color.White);
            //this.spriteBatch.Draw(this.linkSheet, new Rectangle(60, 10, 32, 40), new Rectangle(45, 219, 32, 40), Color.White);


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
                this.DrawHex(this.spriteBatch, blank, color, hex);

                //var text = string.Format("{0},{1}", hex.Position.X, hex.Position.Y);
                //var text = string.Format("{0},{1}", hex.Position.X - 4, (hex.Position.Y - 4) * 2 + hex.Position.X % 2);
                //var text = string.Format("{0},{1}", hex.Position.X - 4, hex.Position.Y - 4);
                var text = string.Format("{0},{1}", hex.Position.X - 4, hex.Position.Y - 5 + (hex.Position.X % 2) * .5);
                //var text = string.Format("{0},{1}", hex.Position.X - 4, hex.Position.Y - 4 + (hex.Position.X % 2) * .5);
                //var text = string.Format("{0},{1}", hex.Position.X - 5, hex.Position.Y - 5 - ((hex.Position.X + 1) % 2) * .5);
                //var text = string.Format("{0}", HexGrid.HexDistance(hexMap[3,3], hex));
                var measure = this.courierNew.MeasureString(text);

                this.spriteBatch.DrawString(this.courierNew, text, hex.Center - (measure / 2.0f), Color.Yellow);
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

        private void DrawHex(SpriteBatch batch, Texture2D blank, Color color, HexGridElement hex)
        {
            var vertices = new List<Vector2>(hex.GetVertices());
            vertices.Add(vertices.First());

            for (var i = 0; i < 6; i++)
            {
                this.DrawLine(batch, blank, 3, color, vertices[i], vertices[i + 1]);
            }
        }

        //private IEnumerable<Vector2> HexCenters(float hexSide, float area)
        //{
        //    //var centerDistance = (float)Math.Sqrt(Math.Pow(hexSide * 2, 2) + Math.Pow(hexSide, 2));
        //    var centerDistance = (float)Math.Sqrt(Math.Pow(hexSide * 2, 2) - Math.Pow(hexSide, 2));
        //    return this.GetAdjacentHexes(new Vector2(1, 1), centerDistance, area);
        //}

        //private IEnumerable<Vector2> GetAdjacentHexes(Vector2 hexCenter, float hexDistance, float area)
        //{
        //    var angles = new[] { -30, 30, 90 };
        //    var hexCenters = new HashSet<Vector2>();
        //    var toExplore = new HashSet<Vector2>();
        //    toExplore.Add(new Vector2(1, 1));

        //    while (toExplore.Any())
        //    {
        //        var hex = toExplore.First();
        //        toExplore.Remove(hex);

        //        foreach (var angle in angles)
        //        {
        //            var adjacent = GetAdjacentHex(hex, angle, hexDistance, area);

        //            if (adjacent.X >= 0 && adjacent.Y >= 0 && adjacent.X <= area && adjacent.Y <= area)
        //            {
        //                if (!toExplore.Contains(adjacent) && !hexCenters.Contains(adjacent))
        //                    toExplore.Add(adjacent);
        //            }
        //        }

        //        hexCenters.Add(hex);
        //    }

        //    return hexCenters;
        //}

        //private Vector2 GetAdjacentHex(Vector2 hexCenter, float angleInDegree, float hexDistance, float area)
        //{
        //    var a1 = AngleToRad(angleInDegree);
        //    var c1 = RoundTo(hexCenter + new Vector2((float)Math.Cos(a1) * hexDistance, (float)Math.Sin(a1) * hexDistance), 1);

        //    return c1;
        //}

        //private static Vector2 RoundTo(Vector2 input, int precision)
        //{
        //    return new Vector2((float)Math.Round(input.X, precision), (float)Math.Round(input.Y, precision));
        //}

        //private static float AngleToRad(float angle)
        //{
        //    return (float)((angle / 360) * Math.PI * 2);
        //}
    }
}
