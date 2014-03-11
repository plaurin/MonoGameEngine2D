using System;
using System.Collections.Generic;
using System.Linq;
using GameFramework;
using GameFramework.Cameras;
using GameFramework.Drawing;
using GameFramework.Inputs;
using GameFramework.IO;
using GameFramework.Layers;
using GameFramework.Scenes;
using GameFramework.Screens;
using GameFramework.Tiles;

namespace SamplesBrowser.Tiled
{
    public class TiledScreen : SceneBasedScreen
    {
        private const float CameraSpeed = 200;
        private readonly ScreenNavigation screenNavigation;
        private Vector position;
        private IEnumerable<TileLayer> tileLayers;

        public TiledScreen(ScreenNavigation screenNavigation)
        {
            this.screenNavigation = screenNavigation;
            this.position = Vector.Zero;
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

            inputConfiguration.AddDigitalButton("CameraLeft").Assign(KeyboardKeys.Left)
                .MapTo(gt => this.MoveCamera(gt.ElapsedSeconds * -CameraSpeed, 0));
            inputConfiguration.AddDigitalButton("CameraRight").Assign(KeyboardKeys.Right)
                .MapTo(gt => this.MoveCamera(gt.ElapsedSeconds * CameraSpeed, 0));
            inputConfiguration.AddDigitalButton("CameraUp").Assign(KeyboardKeys.Up)
                .MapTo(gt => this.MoveCamera(0, gt.ElapsedSeconds * -CameraSpeed));
            inputConfiguration.AddDigitalButton("CameraDown").Assign(KeyboardKeys.Down)
                .MapTo(gt => this.MoveCamera(0, gt.ElapsedSeconds * CameraSpeed));
            inputConfiguration.AddDigitalButton("ZoomIn").Assign(KeyboardKeys.Add).Assign(KeyboardKeys.OemPlus)
                .MapTo(gt => this.Camera.ZoomFactor += gt.ElapsedSeconds);
            inputConfiguration.AddDigitalButton("ZoomOut").Assign(KeyboardKeys.OemMinus)
                .MapTo(gt => this.Camera.ZoomFactor -= gt.ElapsedSeconds);

            inputConfiguration.AddDigitalButton("SwitchSampler").Assign(KeyboardKeys.Tab)
                .MapClickTo(gt => { foreach (var l in this.tileLayers) l.UseLinearSampler = !l.UseLinearSampler; });

            return inputConfiguration;
        }

        protected override Scene CreateScene()
        {
            var scene = new Scene("Tiled") { UseLinearSampler = false };
            scene.Add(new ColorLayer("Background", Color.CornflowerBlue));

            var tiledFile = TiledFile.Load(@"Tiled\untitled.tmx", this.ResourceManager);
            this.tileLayers = tiledFile.TileLayers.ToList();
            foreach (var l in this.tileLayers) l.UseLinearSampler = true;
            scene.AddRange(this.tileLayers);

            var drawingLayer = new DrawingLayer("ObjectLayer");
            foreach (var tiledObject in tiledFile.ObjectLayers.First().TiledObjects)
                drawingLayer.AddRectangle(tiledObject.Position.X, tiledObject.Position.Y, 10, 10, 2, Color.Red);

            scene.Add(drawingLayer);

            return scene;
        }

        private void MoveCamera(float offsetX, float offsetY)
        {
            this.position += new Vector(offsetX, offsetY);
            this.Camera.MoveTo((int)Math.Round(this.position.X), (int)Math.Round(this.position.Y));
        }
    }
}