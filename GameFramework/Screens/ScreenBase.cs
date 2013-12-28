using System;
using System.Collections.Generic;
using GameFramework.Cameras;
using GameFramework.Inputs;
using GameFramework.Scenes;

namespace GameFramework.Screens
{
    public abstract class ScreenBase
    {
        public abstract void Initialize(Camera camera);

        public abstract InputConfiguration GetInputConfiguration();

        public abstract void LoadContent(GameResourceManager theResourceManager);

        public abstract void Update(IGameTiming gameTime);

        public abstract Scene GetScene();
    }

    public class ScreenNavigation
    {
        private readonly Func<Camera> cameraFactory;
        private readonly IDictionary<Type, ScreenContext> screens;
        private GameResourceManager gameResourceManager;

        public ScreenNavigation(Func<Camera> cameraFactory)
        {
            this.cameraFactory = cameraFactory;

            this.screens = new Dictionary<Type, ScreenContext>();
        }

        public void AddScreen(ScreenBase screen)
        {
            this.screens.Add(screen.GetType(), new ScreenContext { Screen = screen });
        }

        public void NavigateTo<T>() where T : ScreenBase
        {
            this.NavigateTo(typeof(T));
        }

        public void NavigateTo(Type screenType)
        {
            var screenToNavigate = this.screens[screenType];

            if (!screenToNavigate.IsInitialized)
            {
                // Init here
                this.Initialize(screenToNavigate);
            }

            this.Current = screenToNavigate;
        }

        public ScreenContext Current { get; set; }

        public void LoadContent(GameResourceManager theGameResourceManager)
        {
            this.gameResourceManager = theGameResourceManager;
        }

        private void Initialize(ScreenContext screenContext)
        {
            // Initialize
            screenContext.Camera = this.cameraFactory.Invoke();
            screenContext.Screen.Initialize(screenContext.Camera);

            screenContext.IsInitialized = true;

            screenContext.InputConfiguration = screenContext.Screen.GetInputConfiguration();

            // LoadContent
            screenContext.Screen.LoadContent(this.gameResourceManager);

            screenContext.Scene = screenContext.Screen.GetScene();
        }
    }

    public class ScreenContext
    {
        public ScreenBase Screen { get; set; }

        public bool IsInitialized { get; set; }

        public Camera Camera { get; set; }

        public InputConfiguration InputConfiguration { get; set; }

        public Scene Scene { get; set; }
    }
}