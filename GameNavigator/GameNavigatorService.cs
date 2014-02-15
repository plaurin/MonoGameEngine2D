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
        private NavigatorViewModel navigatorViewModel;

        public void Launch(IScreen gameScreen)
        {
            // Create a thread
            var newWindowThread = new Thread(() =>
            {
                this.navigatorViewModel = new NavigatorViewModel(gameScreen);

                var navigator = new Navigator { DataContext = navigatorViewModel };
                this.window = new Window { Content = navigator, Width = 300, Left = 0 };
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

        public NavigatorMessage Update(IGameTiming gameTiming)
        {
            if (this.window != null)
            {
                NavigatorMessage result = null;

                this.window.Dispatcher.Invoke(() =>
                {
                    result = this.navigatorViewModel.Update(gameTiming);
                });

                return result;
            }

            return new NavigatorMessage();
        }
    }
}
