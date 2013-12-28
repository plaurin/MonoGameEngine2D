using System;

using GameFramework;
using GameFramework.Cameras;
using GameFramework.Inputs;
using GameFramework.Scenes;
using GameFramework.Screens;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Framework;
using MonoGameImplementation.EngineImplementation;
using Color = Microsoft.Xna.Framework.Color;

namespace MonoGameImplementation
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public abstract class MonoGameBase : Game
    {
        private readonly GraphicsDeviceManager graphics;

        private readonly Type initialScreen;

        //private readonly ScreenBase screen;

        private SpriteBatch spriteBatch;

        //private Camera camera;

        //private float elapseTime;

        //private long frameCounter;

        //private long fps;

        private GameResourceManager gameResourceManager;

        //private InputConfiguration inputConfiguration;

        //private Scene scene;

        private readonly GameFramework.GameTimer gameTimer;

        private readonly ScreenNavigation screenNavigation;

        protected MonoGameBase(ScreenBase initialScreen)
        {
            this.graphics = new GraphicsDeviceManager(this);
            this.Content.RootDirectory = "Content";
            //this.screen = initialScreen;
            this.initialScreen = initialScreen.GetType();
            this.gameTimer = new GameFramework.GameTimer();

            this.screenNavigation = new ScreenNavigation(this.CreateCamera);
            this.screenNavigation.AddScreen(initialScreen);
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
            //this.camera = XnaCamera.CreateCamera(this.graphics.GraphicsDevice.Viewport);
            //this.camera = this.CreateCamera();
            
            //this.screen.Initialize(this.camera);

            //this.inputConfiguration = this.screen.GetInputConfiguration();

            base.Initialize();
        }

        public Camera CreateCamera()
        {
            var viewport = new GameFramework.Viewport(this.graphics.GraphicsDevice.Viewport.Width, 
                this.graphics.GraphicsDevice.Viewport.Height);
            return new Camera(viewport);

            //return XnaCamera.CreateCamera(this.graphics.GraphicsDevice.Viewport);
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
            this.screenNavigation.LoadContent(this.gameResourceManager);
            this.screenNavigation.NavigateTo(this.initialScreen);

            //this.screen.LoadContent(this.gameResourceManager);

            //this.scene = this.screen.GetScene();
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
            this.gameTimer.Update(gameTime.ElapsedGameTime, gameTime.TotalGameTime);

            // FPS
            //this.elapseTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            //this.frameCounter++;

            //if (this.elapseTime > 1)
            //{
            //    this.fps = this.frameCounter;
            //    this.frameCounter = 0;
            //    this.elapseTime = 0;
            //}

            // Allows the game to exit

            // TODO: Allow to exit another way!
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here
//            this.inputConfiguration.Update(new XnaInputContext(), this.gameTimer);
            this.screenNavigation.Current.InputConfiguration.Update(new XnaInputContext(), this.gameTimer);

            this.screenNavigation.Current.Screen.Update(this.gameTimer);

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

            var blank = new Texture2D(this.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            blank.SetData(new[] { Color.White });

            var drawContext = new XnaDrawContext(this.spriteBatch, blank, this.graphics.GraphicsDevice.Viewport);

            //this.scene.Draw(drawContext, this.camera);
            this.screenNavigation.Current.Scene.Draw(drawContext, this.screenNavigation.Current.Camera);

            // TODO: Move this option elsewhere
            //this.DrawCamera(drawContext);

            this.spriteBatch.End();

            base.Draw(gameTime);
        }

        //private void DrawCamera(DrawContext drawContext)
        //{
        //    drawContext.DrawLine(
        //        this.camera.ViewPortCenter.Translate(-10, 0).ToVector(),
        //        this.camera.ViewPortCenter.Translate(10, 0).ToVector(), 1.0f, new GameFramework.Color(255, 255, 0, 255));

        //    drawContext.DrawLine(
        //        this.camera.ViewPortCenter.Translate(0, -10).ToVector(),
        //        this.camera.ViewPortCenter.Translate(0, 10).ToVector(), 1.0f, new GameFramework.Color(255, 255, 0, 255));
        //}
    }
}
