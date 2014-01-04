using MonoGameImplementation;
using SamplesBrowser;
using SamplesBrowser.Sandbox;

namespace Windows8Samples
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class SamplesBrowserWin8 : MonoGameBase
    {
        public SamplesBrowserWin8()
            : base(new SampleBrowserScreenNavigation())
        {
        }
    }
}
