using System;
using System.Collections.Generic;
using System.Linq;
using GameFramework.Inputs;

namespace GameFramework.Screens
{
    public class ScreenNavigation
    {
        private readonly IDictionary<Type, ScreenContext> screens;

        private readonly Stack<ScreenContext> navigationStack;

        private GameResourceManager gameResourceManager;

        private ScreenContext current;

        public ScreenNavigation()
        {
            this.screens = new Dictionary<Type, ScreenContext>();
            this.navigationStack = new Stack<ScreenContext>();
        }

        public Type InitialScreen { get; private set; }

        public bool ShouldExit { get; private set; }

        public bool IsEnabledGesturesUpdated
        {
            get { return this.current.IsEnabledGesturesUpdated; }
            set { this.current.IsEnabledGesturesUpdated = value; }
        }

        public IEnumerable<TouchGestureType> EnabledGestures
        {
            get { return this.current.EnabledGestures; }
        }

        public void Initialize(Viewport viewPort)
        {
            foreach (var screenContext in this.screens.Values.Where(screenContext => !screenContext.IsInitialized))
            {
                Initialize(screenContext, viewPort);
            }
        }

        public void LoadContent(GameResourceManager theGameResourceManager)
        {
            this.gameResourceManager = theGameResourceManager;
        }

        public void Update()
        {
            if (this.navigationStack != null && this.current != this.navigationStack.Peek())
                this.current = this.navigationStack.Peek();
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

            this.navigationStack.Push(screenToNavigate);
        }

        public void NavigateBack()
        {
            this.navigationStack.Pop();
        }

        public void Exit()
        {
            this.ShouldExit = true;
        }

        public void Update(InputContext inputContext, IGameTiming gameTime)
        {
            this.current.Update(inputContext, gameTime);
        }

        public int Draw(DrawContext drawContext)
        {
            return this.current.Draw(drawContext);
        }

        protected void AddScreen(params ScreenBase[] screensToAdd)
        {
            foreach (var screen in screensToAdd)
            {
                this.screens.Add(screen.GetType(), new ScreenContext(screen));
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

        private static void Initialize(ScreenContext screenContext, Viewport viewPort)
        {
            screenContext.Initialize(viewPort);

            screenContext.IsInitialized = true;
        }

        private void LoadContent(ScreenContext screenContext)
        {
            screenContext.LoadContent(this.gameResourceManager);

            screenContext.IsContentLoaded = true;
        }
    }
}