using System;

using GameFramework;
using GameFramework.Cameras;
using GameFramework.Screens;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
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

        private readonly GameFramework.GameTimer gameTimer;

        private readonly ScreenNavigation screenNavigation;

        private SpriteBatch spriteBatch;

        private GameResourceManager gameResourceManager;

        protected MonoGameBase(ScreenBase initialScreen)
            : this(new SingleScreenNavigation(initialScreen))
        {
        }

        protected MonoGameBase(ScreenNavigation screenNavigation)
        {
            this.graphics = new GraphicsDeviceManager(this);
            this.Content.RootDirectory = "Content";

            this.gameTimer = new GameFramework.GameTimer();

            this.screenNavigation = screenNavigation;

            this.initialScreen = screenNavigation.InitialScreen;
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
            this.screenNavigation.Initialize(this.CreateCamera);

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
            this.screenNavigation.LoadContent(this.gameResourceManager);
            this.screenNavigation.NavigateTo(this.initialScreen);
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

            // Allows the game to exit
            if (this.screenNavigation.ShouldExit) this.Exit();

            this.screenNavigation.Update();

            // TODO: Review this if we decide to implement Gestures ourself
            if (this.screenNavigation.Current.InputConfiguration.EnabledGesturesUpdated)
            {
                TouchPanel.EnabledGestures = 
                    XnaInputContext.GetGestures(this.screenNavigation.Current.InputConfiguration.EnabledGestures);

                this.screenNavigation.Current.InputConfiguration.EnabledGesturesUpdated = false;
            }

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
            this.gameTimer.DrawFrame(gameTime.ElapsedGameTime, gameTime.TotalGameTime);

            this.GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            this.spriteBatch.Begin();

            var blank = new Texture2D(this.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            blank.SetData(new[] { Color.White });

            var xnaDrawContext = new XnaDrawContext(this.spriteBatch, blank, this.graphics.GraphicsDevice.Viewport);
            var drawContext = new DrawContextWithCamera(xnaDrawContext, this.screenNavigation.Current.Camera);

            this.screenNavigation.Current.Screen.Draw(drawContext);

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

        private Camera CreateCamera()
        {
            var viewport = new GameFramework.Viewport(
                this.graphics.GraphicsDevice.Viewport.Width,
                this.graphics.GraphicsDevice.Viewport.Height);

            return new Camera(viewport);
        }

        private class SingleScreenNavigation : ScreenNavigation
        {
            public SingleScreenNavigation(ScreenBase screen)
            {
                this.AddScreen(screen);
                this.SetInitialScreen(screen);
            }
        }
    }
}
