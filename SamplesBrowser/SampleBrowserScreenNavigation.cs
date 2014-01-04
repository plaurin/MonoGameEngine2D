using GameFramework.Screens;
using SamplesBrowser.Sandbox;
using SamplesBrowser.ShootEmUp;

namespace SamplesBrowser
{
    public class SampleBrowserScreenNavigation : ScreenNavigation
    {
        public SampleBrowserScreenNavigation()
        {
            this.AddScreen(new HubScreen(), new SandboxScreen(), new ShootEmUpScreen());

            this.SetInitialScreen<HubScreen>();
        }
    }
}