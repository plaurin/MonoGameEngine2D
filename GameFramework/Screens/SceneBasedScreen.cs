using System.Collections.Generic;
using GameFramework.Cameras;
using GameFramework.Inputs;
using GameFramework.Scenes;

namespace GameFramework.Screens
{
    public abstract class SceneBasedScreen : ScreenBase, IComposite, IUpdatable, IDrawable
    {
        public IEnumerable<object> Children
        {
            get { return new[] { this.Scene }; }
        }

        protected Camera Camera { get; private set; }

        protected InputConfiguration InputConfiguration { get; private set; }

        protected GameResourceManager ResourceManager { get; private set; }

        protected Scene Scene { get; private set; }

        public override void Initialize(Viewport viewport)
        {
            this.Camera = this.CreateCamera(viewport);
            this.InputConfiguration = this.CreateInputConfiguration();
        }

        public override void LoadContent(GameResourceManager gameResourceManager)
        {
            this.ResourceManager = gameResourceManager;
            this.Scene = this.CreateScene();
        }

        public override void Update(InputContext inputContext, IGameTiming gameTime)
        {
            this.InputConfiguration.Update(inputContext, gameTime);
            this.Update(gameTime);
        }

        public override int Draw(DrawContext drawContext)
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

        protected abstract Camera CreateCamera(Viewport viewport);

        protected abstract InputConfiguration CreateInputConfiguration();

        protected abstract Scene CreateScene();
    }
}