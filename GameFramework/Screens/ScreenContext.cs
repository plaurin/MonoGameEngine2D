using System;
using GameFramework.Cameras;
using GameFramework.Inputs;
using GameFramework.Scenes;

namespace GameFramework.Screens
{
    // TODO: Should remove this... Tell don't ask
    public class ScreenContext
    {
        public ScreenBase Screen { get; set; }

        public bool IsInitialized { get; set; }

        public bool IsContentLoaded { get; set; }

        public InputConfiguration InputConfiguration { get; set; }

        public Scene Scene { get; set; }
    }
}