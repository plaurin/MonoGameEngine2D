using System;
using System.Collections.Generic;
using GameFramework.Inputs;

namespace GameFramework.Screens
{
    // TODO: Should remove this... Tell don't ask
    public class ScreenContext
    {
        private readonly ScreenBase screen;
        private readonly IEnumerable<TouchGestureType> touchGestures;

        public ScreenContext(ScreenBase screen)
        {
            this.screen = screen;

            var touchEnabled = this.screen as ITouchEnabled;
            if (touchEnabled != null)
            {
                this.IsEnabledGesturesUpdated = true;
                this.touchGestures = touchEnabled.TouchGestures;
            }
        }

        public bool IsInitialized { get; set; }

        public bool IsContentLoaded { get; set; }

        public bool IsEnabledGesturesUpdated { get; set; }

        public IEnumerable<TouchGestureType> EnabledGestures
        {
            get { return this.touchGestures; }
        }

        public void Initialize(Viewport viewport)
        {
            this.screen.Initialize(viewport);
        }

        public void LoadContent(GameResourceManager theResourceManager)
        {
            this.screen.LoadContent(theResourceManager);
        }

        public void Update(InputContext inputContext, IGameTiming gameTime)
        {
            this.screen.Update(inputContext, gameTime);
        }

        public int Draw(DrawContext drawContext)
        {
            return this.screen.Draw(drawContext);
        }
    }
}