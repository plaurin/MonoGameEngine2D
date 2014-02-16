using System;
using System.Reflection;
using GameFramework;
using GameFramework.Screens;
using GameNavigator;
using Microsoft.Xna.Framework;

namespace MonoGameImplementation
{
    public class GameNavigatorGateway
    {
        private readonly GameNavigatorService service;

        public GameNavigatorGateway()
        {
            this.service = new GameNavigatorService();
        }

        public bool IsOpen
        {
            get { return this.service.IsNavigatorOpen; }
        }

        public void Launch(IScreen screen, GameWindow gameWindow)
        {
            //gameWindow.AllowUserResizing = true;

            this.HookWindowClosingShouldSavePosition(gameWindow);

            this.service.Launch(screen, (x, y) => MoveGameWindow(gameWindow, x, y));
        }

        public NavigatorMessage Update(IGameTiming gameTiming)
        {
            return this.service.Update(gameTiming);
        }

        public void Show()
        {
            this.service.Show();
        }

        private static void MoveGameWindow(GameWindow gameWindow, int x, int y)
        {
            var field = typeof(OpenTKGameWindow).GetField("window", BindingFlags.NonPublic | BindingFlags.Instance);
            var window = (OpenTK.GameWindow)field.GetValue(gameWindow);
            window.X = x;
            window.Y = y;
        }

        private void HookWindowClosingShouldSavePosition(GameWindow gameWindow)
        {
            var field = typeof(OpenTKGameWindow).GetField("window", BindingFlags.NonPublic | BindingFlags.Instance);
            var window = (OpenTK.GameWindow)field.GetValue(gameWindow);

            window.Closing += (sender, args) => this.service.PersistGameWindowPosition(window.X, window.Y);
        }
    }
}