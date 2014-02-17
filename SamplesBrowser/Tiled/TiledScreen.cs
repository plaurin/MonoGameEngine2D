using System;
using System.Linq;
using GameFramework;
using GameFramework.Cameras;
using GameFramework.Drawing;
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

            var tiledFile = TiledFile.Load(@"Tiled\untitled.tmx", this.ResourceManager);
            scene.AddRange(tiledFile.TileLayers);

            var drawingLayer = new DrawingLayer("ObjectLayer");
            foreach (var tiledObject in tiledFile.ObjectLayers.First().TiledObjects)
                drawingLayer.AddRectangle(tiledObject.Position.X, tiledObject.Position.Y, 10, 10, 2, Color.Red);

            scene.Add(drawingLayer);

            return scene;
        }
    }
}