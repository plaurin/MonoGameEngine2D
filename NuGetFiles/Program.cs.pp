using System;
using MonoGameImplementation;

namespace $rootnamespace$
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            using (var game = new MonoGameBase(new DefaultScreen())) game.Run();
        }
    }
}