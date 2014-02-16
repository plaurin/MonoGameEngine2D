using System;
using System.Collections.Generic;
using System.Linq;
using GameFramework;
using GameFramework.Cameras;
using GameFramework.Drawing;
using GameFramework.Inputs;
using GameFramework.Scenes;
using GameFramework.Screens;
using GameFramework.Utilities;
using SamplesBrowser.Sandbox;
using SamplesBrowser.ShootEmUp;
using SamplesBrowser.Tiled;
using SamplesBrowser.Touch;

namespace SamplesBrowser
{
    public class HubScreen : SceneBasedScreen, ITouchEnabled
    {
        private readonly ScreenNavigation screenNavigation;

        private Samples hoveringSample = Samples.None;
        private Samples currentSample = Samples.None;

        private RectangleElement sandboxRectangle;
        private RectangleElement shootEmUpRectangle;
        private RectangleElement tiledRectangle;
        private RectangleElement touchRectangle;

        private MouseStateBase mouseState;
        private TouchStateBase touchState;

        public HubScreen(ScreenNavigation screenNavigation)
        {
            this.screenNavigation = screenNavigation;
        }

        private enum Samples
        {
            None,
            Sandbox,
            ShootEmUp,
            Tiled,
            Touch
        }

        public IEnumerable<TouchGestureType> TouchGestures
        {
            get { yield return TouchGestureType.Tap; }
        }

        public override void Update(IGameTiming gameTime)
        {
            Func<Samples, Color> colorFunc = sample =>
                this.hoveringSample == sample ? Color.Blue : this.currentSample == sample ? Color.Red : Color.White;

            this.sandboxRectangle.Color = colorFunc(Samples.Sandbox);
            this.shootEmUpRectangle.Color = colorFunc(Samples.ShootEmUp);
            this.tiledRectangle.Color = colorFunc(Samples.Tiled);
            this.touchRectangle.Color = colorFunc(Samples.Touch);
        }

        protected override Camera CreateCamera(Viewport viewport)
        {
            return new Camera(viewport) { Center = CameraCenter.WindowTopLeft };
        }

        protected override InputConfiguration CreateInputConfiguration()
        {
            var inputConfiguration = new InputConfiguration();

            // Keyboard
            inputConfiguration.AddDigitalButton("Back").Assign(KeyboardKeys.Escape)
                //.MapClickTo(gt => this.screenNavigation.Exit());
                .MapClickTo(gt => this.Exit());

            inputConfiguration.AddDigitalButton("GotoSandbox").Assign(KeyboardKeys.D1)
                .MapClickTo(gt => this.LaunchSandboxSample());

            inputConfiguration.AddDigitalButton("GotoShootEmUp").Assign(KeyboardKeys.D2)
                .MapClickTo(gt => this.LaunchShootEmUpSample());

            inputConfiguration.AddDigitalButton("GotoTiled").Assign(KeyboardKeys.D3)
                .MapClickTo(gt => this.LaunchTiledSample());

            inputConfiguration.AddDigitalButton("GotoTouch").Assign(KeyboardKeys.D4)
                .MapClickTo(gt => this.LaunchTouchSample());

            // Mouse
            Func<RectangleHit, Samples> hitToSampleFunc = hit =>
            {
                if (hit != null)
                {
                    if (hit.RectangleElement == this.sandboxRectangle) return Samples.Sandbox;
                    if (hit.RectangleElement == this.shootEmUpRectangle) return Samples.ShootEmUp;
                    if (hit.RectangleElement == this.tiledRectangle) return Samples.Tiled;
                    if (hit.RectangleElement == this.touchRectangle) return Samples.Touch;
                }

                return Samples.None;
            };

            inputConfiguration.CreateMouseTracking(this.Camera).OnMove((mt, e) =>
            {
                this.mouseState = mt;

                var hit = this.Scene.GetHits(this.mouseState.AbsolutePosition, this.Camera).OfType<RectangleHit>().FirstOrDefault();
                this.hoveringSample = hitToSampleFunc(hit);
            });

            inputConfiguration.AddDigitalButton("MouseSelection").Assign(MouseButtons.Left).MapClickTo(elapse =>
            {
                var hit = this.Scene.GetHits(this.mouseState.AbsolutePosition, this.Camera).OfType<RectangleHit>().FirstOrDefault();
                var hitSample = hitToSampleFunc(hit);

                switch (hitSample)
                {
                    case Samples.Sandbox: 
                        this.LaunchSandboxSample();
                        break;
                    case Samples.ShootEmUp:
                        this.LaunchShootEmUpSample();
                        break;
                    case Samples.Tiled:
                        this.LaunchTiledSample();
                        break;
                    case Samples.Touch:
                        this.LaunchTouchSample();
                        break;
                }
            });

            // Touch
            inputConfiguration.CreateTouchTracking(this.Camera).OnTouch((ts, gt) =>
            {
                this.touchState = ts;

                if (this.touchState.Touches.Any())
                {
                    var hit = this.touchState.Touches
                        .SelectMany(t => this.Scene.GetHits(t.Position, this.Camera))
                        .OfType<RectangleHit>().FirstOrDefault();

                    this.hoveringSample = hitToSampleFunc(hit);
                }
            });

            inputConfiguration.AddEvent("TouchSelection").Assign(TouchGestureType.Tap).MapTo(gt =>
            {
                var hit = this.Scene.GetHits(this.touchState.CurrentGesture.Position, this.Camera)
                    .OfType<RectangleHit>().FirstOrDefault();

                var hitSample = hitToSampleFunc(hit);

                switch (hitSample)
                {
                    case Samples.Sandbox:
                        this.LaunchSandboxSample();
                        break;
                    case Samples.ShootEmUp:
                        this.LaunchShootEmUpSample();
                        break;
                    case Samples.Tiled:
                        this.LaunchTiledSample();
                        break;
                    case Samples.Touch:
                        this.LaunchTouchSample();
                        break;
                }
            });

            return inputConfiguration;
        }

        protected override Scene CreateScene()
        {
            // TODO: Fix this... This is weird, mendatory??
            this.ResourceManager.AddDrawingFont(@"Sandbox\SpriteFont1");

            var scene = new Scene("HubScene");

            var font = this.ResourceManager.GetDrawingFont(@"Sandbox\SpriteFont1");

            var hubMap = new DrawingLayer("HubMap");

            this.sandboxRectangle = hubMap.AddRectangle(10, 10, 200, 200, 1, Color.White);
            this.shootEmUpRectangle = hubMap.AddRectangle(220, 10, 200, 200, 1, Color.White);
            this.tiledRectangle = hubMap.AddRectangle(10, 230, 200, 200, 1, Color.White);
            this.touchRectangle = hubMap.AddRectangle(220, 230, 200, 200, 1, Color.White);
            //hubMap.AddRectangle(10, 230, 200, 200, 1, Color.White);
            //hubMap.AddRectangle(220, 230, 200, 200, 1, Color.White);

            hubMap.AddText(font, "1 - Sandbox sample", new Vector(20, 210), Color.White);
            hubMap.AddText(font, "2 - ShootEmUp sample", new Vector(220, 210), Color.White);
            hubMap.AddText(font, "3 - Tiled sample", new Vector(20, 430), Color.White);
            hubMap.AddText(font, "4 - Touch sample", new Vector(220, 430), Color.White);

            scene.Add(hubMap);

            //this.mouseLayer = MouseCursorLayer.Create(this.ResourceManager);
            scene.Add(new MouseCursor(this.InputConfiguration.CreateMouseTracking(this.Camera)));

            return scene;
        }

        private void LaunchSandboxSample()
        {
            this.currentSample = Samples.Sandbox;
            this.screenNavigation.NavigateTo<SandboxScreen>();
        }

        private void LaunchShootEmUpSample()
        {
            this.currentSample = Samples.ShootEmUp;
            this.screenNavigation.NavigateTo<ShootEmUpScreen>();
        }

        private void LaunchTiledSample()
        {
            this.currentSample = Samples.Tiled;
            this.screenNavigation.NavigateTo<TiledScreen>();
        }

        private void LaunchTouchSample()
        {
            this.currentSample = Samples.Touch;
            this.screenNavigation.NavigateTo<TouchScreen>();
        }
    }
}
