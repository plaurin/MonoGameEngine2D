using System;
using System.Collections.Generic;
using System.Linq;
using GameFramework.Cameras;

namespace GameFramework.Screens
{
    public class ScreenNavigation
    {
        private readonly IDictionary<Type, ScreenContext> screens;

        private GameResourceManager gameResourceManager;

        public ScreenNavigation()
        {
            this.screens = new Dictionary<Type, ScreenContext>();
        }

        public Type InitialScreen { get; private set; }

        public ScreenContext Current { get; private set; }

        public void Initialize(Func<Camera> cameraFactory)
        {
            foreach (var screenContext in this.screens.Values.Where(screenContext => !screenContext.IsInitialized))
            {
                Initialize(screenContext, cameraFactory.Invoke());
            }
        }

        public void LoadContent(GameResourceManager theGameResourceManager)
        {
            this.gameResourceManager = theGameResourceManager;
        }

        public void NavigateTo<T>() where T : ScreenBase
        {
            this.NavigateTo(typeof(T));
        }

        public void NavigateTo(Type screenType)
        {
            var screenToNavigate = this.screens[screenType];

            if (!screenToNavigate.IsContentLoaded)
            {
                this.LoadContent(screenToNavigate);
            }

            this.Current = screenToNavigate;
        }

        protected void AddScreen(params ScreenBase[] screensToAdd)
        {
            foreach (var screen in screensToAdd)
            {
                this.screens.Add(screen.GetType(), new ScreenContext { Screen = screen });
            }
        }

        protected void SetInitialScreen<TScreen>() where TScreen : ScreenBase
        {
            this.InitialScreen = typeof(TScreen);
        }

        protected void SetInitialScreen(ScreenBase screen)
        {
            this.InitialScreen = screen.GetType();
        }

        private static void Initialize(ScreenContext screenContext, Camera camera)
        {
            screenContext.Camera = camera;
            screenContext.Screen.Initialize(camera);
            screenContext.InputConfiguration = screenContext.Screen.GetInputConfiguration();

            screenContext.IsInitialized = true;
        }

        private void LoadContent(ScreenContext screenContext)
        {
            screenContext.Screen.LoadContent(this.gameResourceManager);
            screenContext.Scene = screenContext.Screen.GetScene();

            screenContext.IsContentLoaded = true;
        }
    }
}