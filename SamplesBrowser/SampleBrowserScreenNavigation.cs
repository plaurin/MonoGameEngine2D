using GameFramework.Screens;
using SamplesBrowser.Sandbox;
using SamplesBrowser.ShootEmUp;
using SamplesBrowser.Tiled;
using SamplesBrowser.Touch;

namespace SamplesBrowser
{
    public class SampleBrowserScreenNavigation : ScreenNavigation
    {
        public SampleBrowserScreenNavigation()
        {
            this.AddScreen(
                new HubScreen(this),
                new SandboxScreen(this),
                new ShootEmUpScreen(this),
                new TiledScreen(this),
                new TouchScreen(this));

            this.SetInitialScreen<HubScreen>();
        }
    }
}