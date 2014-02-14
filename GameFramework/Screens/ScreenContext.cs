using System;
using System.Collections.Generic;
using GameFramework.Inputs;

namespace GameFramework.Screens
{
    public class ScreenContext : IScreen
    {
        private readonly IScreen screen;
        private readonly IEnumerable<TouchGestureType> touchGestures;

        private bool isEnabledGesturesUpdated;

        public ScreenContext(IScreen screen)
        {
            this.screen = screen;

            var touchEnabled = this.screen as ITouchEnabled;
            if (touchEnabled != null)
            {
                this.isEnabledGesturesUpdated = true;
                this.touchGestures = touchEnabled.TouchGestures;
            }
        }

        public bool IsInitialized { get; private set; }

        public bool IsContentLoaded { get; private set; }

        public bool ShouldExit
        {
            get { return this.screen.ShouldExit; }
        }

        public void Initialize(Viewport viewport)
        {
            this.screen.Initialize(viewport);
            this.IsInitialized = true;
        }

        public void LoadContent(GameResourceManager theResourceManager)
        {
            this.screen.LoadContent(theResourceManager);
            this.IsContentLoaded = true;
        }

        public void Update(InputContext inputContext, IGameTiming gameTime)
        {
            if (this.isEnabledGesturesUpdated)
            {
                inputContext.UpdateEnabledGestures(this.touchGestures);
                this.isEnabledGesturesUpdated = false;
            }

            this.screen.Update(inputContext, gameTime);
        }

        public int Draw(DrawContext drawContext)
        {
            return this.screen.Draw(drawContext);
        }
    }
}