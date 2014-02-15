using GameFramework;
using GameFramework.Screens;
using GameNavigator;

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

        public void Launch(IScreen screen)
        {
            this.service.Launch(screen);
        }

        public NavigatorMessage Update(IGameTiming gameTiming)
        {
            return this.service.Update(gameTiming);
        }

        public void Show()
        {
            this.service.Show();
        }
    }
}