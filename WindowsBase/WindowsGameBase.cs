using System;
using System.Diagnostics;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using FxBase;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace WindowsBase
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public abstract class WindowsGameBase : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private Texture2D blank;
        private Dictionary<string, Texture2D> textures; 

        protected WindowsGameBase()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            this.textures = new Dictionary<string, Texture2D>();
        }

        public Scene Scene { get; protected set; }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            Debug.Assert(this.Scene != null, "Scene needs to be set in the Game class constructor");

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
            this.blank = new Texture2D(this.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            blank.SetData(new[] { Color.White });

            //this.link = Content.Load<Texture2D>("LinkSheet.png");
            foreach (var sprite in this.Scene.Entities.OfType<Sprite>())
                this.textures.Add(sprite.TexturePath, Content.Load<Texture2D>(sprite.TexturePath));
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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

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

            //spriteBatch.Draw(this.link, new Vector2(10, 10), Color.White);
            foreach (var entity in Scene.Entities)
            {
                var sprite = entity as Sprite;
                if (sprite != null)
                {
                    var texture = this.textures[sprite.TexturePath];
                    var vector = new Vector2(sprite.Position.X, sprite.Position.Y);
                    spriteBatch.Draw(texture, vector, Color.White);
                }

                var line = entity as Line;
                if (line != null)
                {
                    var p1 = new Vector2(line.Point1.X, line.Point1.Y);
                    var p2 = new Vector2(line.Point2.X, line.Point2.Y);

                    var angle = (float)Math.Atan2(p2.Y - p1.Y, p2.X - p1.X);
                    var length = Vector2.Distance(p1, p2);

                    this.spriteBatch.Draw(this.blank, p1, null, Color.White, angle, Vector2.Zero,
                        new Vector2(length, 1), SpriteEffects.None, 0);
                }
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
