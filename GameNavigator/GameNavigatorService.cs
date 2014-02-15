using System;
using System.Threading;
using System.Windows;
using GameFramework;
using GameFramework.Screens;

namespace GameNavigator
{
    public class GameNavigatorService
    {
        private Window window;
        private IScreen screen;

        public void Launch(IScreen gameScreen)
        {
            this.screen = gameScreen;

            // Create a thread
            var newWindowThread = new Thread(() =>
            {
                this.window = new Window { Content = new UserControl1(), Width = 300, Left = 0 };
                this.window.Show();

                // Start the Dispatcher Processing
                System.Windows.Threading.Dispatcher.Run();
            });

            // Set the apartment state
            newWindowThread.SetApartmentState(ApartmentState.STA);
            // Make the thread a background thread
            newWindowThread.IsBackground = true;
            // Start the thread
            newWindowThread.Start();
        }

        public void Update(IGameTiming gameTiming)
        {
            if (this.window != null)
            {
                this.window.Dispatcher.Invoke(() =>
                {
                    //var composite = this.screen as IComposite;
                    //if (composite != null)
                    //{
                    //    var c = composite.Children;

                    //    window.Title = c.Count().ToString();
                    //}

                    window.Title = gameTiming.TotalSeconds.ToString();
                });
            }
        }
    }
}
