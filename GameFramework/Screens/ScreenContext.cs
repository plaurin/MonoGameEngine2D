using GameFramework.Cameras;
using GameFramework.Inputs;
using GameFramework.Scenes;

namespace GameFramework.Screens
{
    public class ScreenContext
    {
        public ScreenBase Screen { get; set; }

        public bool IsInitialized { get; set; }

        public bool IsContentLoaded { get; set; }

        public Camera Camera { get; set; }

        public InputConfiguration InputConfiguration { get; set; }

        public Scene Scene { get; set; }
    }
}