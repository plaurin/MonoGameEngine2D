using System;
using System.Collections.Generic;
using System.Linq;
using GameFramework.Inputs;

namespace GameFramework.Screens
{
    public class ScreenNavigation : IScreen, IComposite
    {
        private readonly IDictionary<Type, ScreenContext> screens;
        private readonly Stack<ScreenContext> navigationStack;

        private GameResourceManager gameResourceManager;
        private ScreenContext current;
        private Type initialScreen;
        private bool shouldExit;

        public ScreenNavigation()
        {
            this.screens = new Dictionary<Type, ScreenContext>();
            this.navigationStack = new Stack<ScreenContext>();
        }

        public IEnumerable<object> Children
        {
            get { return this.screens.Values; }
        }

        public bool ShouldExit
        {
            get { return this.shouldExit || (this.current != null && this.current.ShouldExit); }
        }

        public bool UseLinearSampler
        {
            get { return this.current != null && this.current.UseLinearSampler; }
        }

        public void Initialize(Viewport viewPort)
        {
            foreach (var screenContext in this.screens.Values.Where(screenContext => !screenContext.IsInitialized))
            {
                screenContext.Initialize(viewPort);
            }
        }

        public void LoadContent(GameResourceManager theGameResourceManager)
        {
            this.gameResourceManager = theGameResourceManager;
            this.NavigateTo(this.initialScreen);
        }

        public void Update(InputContext inputContext, IGameTiming gameTime)
        {
            this.CompletePendingTransition();

            this.current.Update(inputContext, gameTime);
        }

        public int Draw(DrawContext drawContext)
        {
            return this.current.Draw(drawContext);
        }

        public void NavigateTo<T>() where T : IScreen
        {
            this.NavigateTo(typeof(T));
        }

        public void NavigateTo(Type screenType)
        {
            var screenToNavigate = this.screens[screenType];

            if (!screenToNavigate.IsContentLoaded)
            {
                screenToNavigate.LoadContent(this.gameResourceManager);
            }

            this.navigationStack.Push(screenToNavigate);
        }

        public void NavigateBack()
        {
            this.navigationStack.Pop();
        }

        public void Exit()
        {
            this.shouldExit = true;
        }

        protected void AddScreen(params IScreen[] screensToAdd)
        {
            foreach (var screen in screensToAdd)
            {
                this.screens.Add(screen.GetType(), new ScreenContext(screen));
            }
        }

        protected void SetInitialScreen<TScreen>() where TScreen : IScreen
        {
            this.initialScreen = typeof(TScreen);
        }

        protected void SetInitialScreen(IScreen screen)
        {
            this.initialScreen = screen.GetType();
        }

        private void CompletePendingTransition()
        {
            if (this.navigationStack != null && this.current != this.navigationStack.Peek())
                this.current = this.navigationStack.Peek();
        }
    }
}