using System;
using MonoGameImplementation;
using SamplesBrowser;

namespace WindowsSamples
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            using (var game = new MonoGameBase(new SampleBrowserScreenNavigation())) game.Run();
        }
    }
}