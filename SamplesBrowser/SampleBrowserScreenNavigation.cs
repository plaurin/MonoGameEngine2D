using GameFramework.Screens;
using SamplesBrowser.Sandbox;
using SamplesBrowser.ShootEmUp;

namespace SamplesBrowser
{
    public class SampleBrowserScreenNavigation : ScreenNavigation
    {
        public SampleBrowserScreenNavigation()
        {
            this.AddScreen(new HubScreen(this), new SandboxScreen(this), new ShootEmUpScreen(this));

            this.SetInitialScreen<HubScreen>();
        }
    }
}