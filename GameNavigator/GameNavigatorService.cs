using System;
using System.Threading;
using System.Windows;
using GameFramework;
using GameFramework.Screens;
using GameNavigator.Properties;

namespace GameNavigator
{
    public class GameNavigatorService
    {
        private Window navigatorWindow;
        private NavigatorViewModel navigatorViewModel;

        public bool IsNavigatorOpen
        {
            get { return this.navigatorWindow == null || this.navigatorWindow.IsVisible; }
        }

        public void Launch(IScreen gameScreen, Action<int, int> moveGameWindow)
        {
            RestoreGameWindow(moveGameWindow);

            // Create a thread
            var newWindowThread = new Thread(() =>
            {
                this.navigatorViewModel = new NavigatorViewModel(gameScreen);
                var navigator = new Navigator { DataContext = navigatorViewModel };
                this.navigatorWindow = new Window { Content = navigator };

                this.RestoreNavigatorWindow();

                this.navigatorWindow.Closing += (sender, args) => { args.Cancel = true; this.navigatorWindow.Hide(); };
                
                this.navigatorWindow.SizeChanged += (sender, args) => PersistNavigatorWindow();
                this.navigatorWindow.LocationChanged += (sender, args) => PersistNavigatorWindow();

                //this.navigatorWindow.Show();

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

        public void Show()
        {
            if (this.navigatorWindow != null)
            {
                this.navigatorWindow.Dispatcher.Invoke(() => this.navigatorWindow.Show());
            }
        }

        public NavigatorMessage Update(IGameTiming gameTiming)
        {
            if (this.navigatorWindow != null)
            {
                NavigatorMessage result = null;

                this.navigatorWindow.Dispatcher.Invoke(() =>
                {
                    result = this.navigatorViewModel.Update(gameTiming);
                    this.navigatorWindow.Title = "Navigator" + (result.ShouldPlay ? string.Empty : " - Pause");
                });

                return result;
            }

            return new NavigatorMessage();
        }

        public void PersistGameWindowPosition(int x, int y)
        {
            Settings.Default.GameWindowPosition = x + "," + y;
            Settings.Default.Save();
        }

        private static void RestoreGameWindow(Action<int, int> moveGameWindow)
        {
            try
            {
                var position = Settings.Default.GameWindowPosition;
                var values = position.Split(',');

                moveGameWindow.Invoke(int.Parse(values[0].Trim()), int.Parse(values[1].Trim()));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private void RestoreNavigatorWindow()
        {
            try
            {
                var position = Settings.Default.NavigatorPosition;
                var values = position.Split(',');

                this.navigatorWindow.Left = int.Parse(values[0].Trim());
                this.navigatorWindow.Top = int.Parse(values[1].Trim());
                this.navigatorWindow.Width = int.Parse(values[2].Trim());
                this.navigatorWindow.Height = int.Parse(values[3].Trim());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private void PersistNavigatorWindow()
        {
            Settings.Default.NavigatorPosition = string.Join(",", new[]
            {
                this.navigatorWindow.Left,
                this.navigatorWindow.Top,
                this.navigatorWindow.Width,
                this.navigatorWindow.Height
            });

            Settings.Default.Save();
        }
    }
}