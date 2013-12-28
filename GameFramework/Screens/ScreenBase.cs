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
        private readonly IDictionary<Type, ScreenContext> screens;

        public ScreenNavigation()
        {
            this.screens = new Dictionary<Type, ScreenContext>();
        }

        public void AddScreen(ScreenBase screen)
        {
            this.screens.Add(screen.GetType(), new ScreenContext());
        }

        public void NavigateTo<T>() where T : ScreenBase
        {
            var screenToNavigate = this.screens[typeof(T)];

            if (!screenToNavigate.IsInitialized)
            {
                // Init here
            }

            this.Current = screenToNavigate;
        }

        public ScreenContext Current { get; set; }
    }

    public class ScreenContext
    {
        public ScreenBase Screen { get; set; }

        public bool IsInitialized { get; set; }

        public void Initialize()
        {
            //this.camera = XnaCamera.CreateCamera(this.graphics.GraphicsDevice.Viewport);

        }
    }
}