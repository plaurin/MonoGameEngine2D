using System;
using GameFramework;
using GameFramework.Cameras;
using GameFramework.Inputs;
using GameFramework.IO;
using GameFramework.Scenes;
using GameFramework.Screens;

namespace SamplesBrowser.Tiled
{
    public class TiledScreen : SceneBasedScreen
    {
        private readonly ScreenNavigation screenNavigation;

        public TiledScreen(ScreenNavigation screenNavigation)
        {
            this.screenNavigation = screenNavigation;
        }

        protected override Camera CreateCamera(Viewport viewport)
        {
            return new Camera(viewport) { Center = CameraCenter.WindowTopLeft };
        }

        protected override InputConfiguration CreateInputConfiguration()
        {
            var inputConfiguration = new InputConfiguration();

            inputConfiguration.AddDigitalButton("Back").Assign(KeyboardKeys.Escape)
                .MapClickTo(gt => this.screenNavigation.NavigateBack());

            return inputConfiguration;
        }

        protected override Scene CreateScene()
        {
            var scene = new Scene("Tiled");

            scene.AddRange(TiledHelper.LoadFile(@"Tiled\untitled.tmx", this.ResourceManager));

            return scene;
        }
    }
}