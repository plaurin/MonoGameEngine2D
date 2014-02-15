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

        public void Launch(IScreen screen)
        {
            this.service.Launch(screen);
        }

        public NavigatorMessage Update(IGameTiming gameTiming)
        {
            return this.service.Update(gameTiming);
        }
    }
}