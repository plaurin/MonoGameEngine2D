using System;
using GameFramework;
using GameFramework.Cameras;
using GameFramework.Inputs;
using GameFramework.IO;
using GameFramework.Scenes;
using GameFramework.Screens;

namespace SamplesBrowser.Tiled
{
    public class TiledScreen : ScreenBase
    {
        private readonly ScreenNavigation screenNavigation;

        private Camera camera;

        private GameResourceManager gameResourceManager;

        private Scene scene;

        private InputConfiguration inputConfiguration;

        public TiledScreen(ScreenNavigation screenNavigation)
        {
            this.screenNavigation = screenNavigation;
        }

        public override void Initialize(Viewport viewport)
        {
            this.camera = new Camera(viewport);
            this.camera.Center = CameraCenter.WindowTopLeft;

            this.CreateInputConfiguration();
        }

        public override void LoadContent(GameResourceManager theResourceManager)
        {
            this.gameResourceManager = theResourceManager;
            this.CreateScene();
        }

        public override void Update(InputContext inputContext, IGameTiming gameTime)
        {
            this.inputConfiguration.Update(inputContext, gameTime);
        }

        public override int Draw(DrawContext drawContext)
        {
            drawContext.Camera = this.camera;
            return this.scene.Draw(drawContext);
        }

        private void CreateInputConfiguration()
        {
            this.inputConfiguration = new InputConfiguration();

            this.inputConfiguration.AddDigitalButton("Back").Assign(KeyboardKeys.Escape)
                .MapClickTo(gt => this.screenNavigation.NavigateBack());
        }

        private void CreateScene()
        {
            this.scene = new Scene("Tiled");

            foreach (var tileMap in TiledHelper.LoadFile(@"Tiled\untitled.tmx", this.gameResourceManager))
                this.scene.Add(tileMap);
        }
    }
}