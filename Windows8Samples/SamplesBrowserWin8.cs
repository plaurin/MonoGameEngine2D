using WindowsBase;
using SamplesBrowser;

namespace Windows8Samples
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class SamplesBrowserWin8 : WindowsGameBase
    {
        public SamplesBrowserWin8()
        {
            this.Scene = new MyScene();
        }
    }
}
