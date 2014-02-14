using System;
using System.Collections.Generic;
using System.Linq;
using GameFramework.Inputs;

namespace GameFramework.Screens
{
    public class ScreenNavigation : ScreenBase
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

        public override bool ShouldExit
        {
            get { return this.shouldExit || (this.current != null && this.current.ShouldExit); }
        }

        public override void Initialize(Viewport viewPort)
        {
            foreach (var screenContext in this.screens.Values.Where(screenContext => !screenContext.IsInitialized))
            {
                screenContext.Initialize(viewPort);
            }
        }

        public override void LoadContent(GameResourceManager theGameResourceManager)
        {
            this.gameResourceManager = theGameResourceManager;
            this.NavigateTo(this.initialScreen);
        }

        public override void Update(InputContext inputContext, IGameTiming gameTime)
        {
            this.CompletePendingTransition();

            this.current.Update(inputContext, gameTime);
        }

        public override int Draw(DrawContext drawContext)
        {
            return this.current.Draw(drawContext);
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

        protected void AddScreen(params ScreenBase[] screensToAdd)
        {
            foreach (var screen in screensToAdd)
            {
                this.screens.Add(screen.GetType(), new ScreenContext(screen));
            }
        }

        protected void SetInitialScreen<TScreen>() where TScreen : ScreenBase
        {
            this.initialScreen = typeof(TScreen);
        }

        protected void SetInitialScreen(ScreenBase screen)
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