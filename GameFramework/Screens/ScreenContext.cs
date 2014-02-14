using System;
using System.Collections.Generic;
using GameFramework.Inputs;
using GameFramework.Scenes;

namespace GameFramework.Screens
{
    // TODO: Should remove this... Tell don't ask
    public class ScreenContext
    {
        private readonly IEnumerable<TouchGestureType> touchGestures;

        public ScreenContext(ScreenBase screenBase)
        {
            this.Screen = screenBase;

            var touchEnabled = this.Screen as ITouchEnabled;
            if (touchEnabled != null)
            {
                this.IsEnabledGesturesUpdated = true;
                this.touchGestures = touchEnabled.TouchGestures;
            }
        }

        public ScreenBase Screen { get; private set; }

        public bool IsInitialized { get; set; }

        public bool IsContentLoaded { get; set; }

        public bool IsEnabledGesturesUpdated { get; set; }

        public IEnumerable<TouchGestureType> EnabledGestures
        {
            get { return this.touchGestures; }
        }

        public Scene Scene { get; set; }
    }
}