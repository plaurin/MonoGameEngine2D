using System.Collections.Generic;
using GameFramework.Cameras;
using GameFramework.Inputs;
using GameFramework.Scenes;

namespace GameFramework.Screens
{
    public abstract class SceneBasedScreen : IScreen, IComposite, IUpdatable, IDrawable, INavigatorMetadataProvider
    {
        private bool shouldExit;

        public IEnumerable<object> Children
        {
            get { return new[] { this.Scene }; }
        }
        
        bool IScreen.ShouldExit
        {
            get { return this.shouldExit; }
        }

        protected Camera Camera { get; private set; }

        protected InputConfiguration InputConfiguration { get; private set; }

        protected GameResourceManager ResourceManager { get; private set; }

        protected Scene Scene { get; private set; }

        void IScreen.Initialize(Viewport viewport)
        {
            this.Camera = this.CreateCamera(viewport);
            this.InputConfiguration = this.CreateInputConfiguration();
        }

        void IScreen.LoadContent(GameResourceManager gameResourceManager)
        {
            this.ResourceManager = gameResourceManager;
            this.Scene = this.CreateScene();
        }

        void IScreen.Update(InputContext inputContext, IGameTiming gameTime)
        {
            this.InputConfiguration.Update(inputContext, gameTime);
            this.Scene.Update(gameTime);

            this.Update(gameTime);
        }

        int IScreen.Draw(DrawContext drawContext)
        {
            drawContext.Camera = this.Camera;
            var total = this.Scene.Draw(drawContext);

            return total + this.Draw(drawContext);
        }

        public virtual void Update(IGameTiming gameTiming)
        {
        }

        public virtual int Draw(IDrawContext drawContext)
        {
            return 0;
        }

        public NavigatorMetadata GetMetadata()
        {
            return new NavigatorMetadata(this.GetType().Name, NodeKind.Screen);
        }

        protected void Exit()
        {
            this.shouldExit = true;
        }

        protected abstract Camera CreateCamera(Viewport viewport);

        protected abstract InputConfiguration CreateInputConfiguration();

        protected abstract Scene CreateScene();
    }
}