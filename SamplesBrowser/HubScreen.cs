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
    public class HubScreen : ScreenBase, ITouchEnabled
    {
        private readonly ScreenNavigation screenNavigation;

        private Camera camera;
        private GameResourceManager gameResourceManager;

        private Samples hoveringSample = Samples.None;
        private Samples currentSample = Samples.None;

        private RectangleElement sandboxRectangle;
        private RectangleElement shootEmUpRectangle;
        private RectangleElement tiledRectangle;
        private RectangleElement touchRectangle;

        private MouseStateBase mouseState;
        private TouchStateBase touchState;

        private Scene scene;
        private InputConfiguration inputConfiguration;

        private MouseCursorLayer mouseLayer;

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

        public override void Initialize(Viewport viewport)
        {
            this.camera = new Camera(viewport);
            this.camera.Center = CameraCenter.WindowTopLeft;

            this.CreateInputConfiguration();
        }

        public IEnumerable<TouchGestureType> TouchGestures
        {
            get { yield return TouchGestureType.Tap; }
        }

        public override void LoadContent(GameResourceManager theResourceManager)
        {
            this.gameResourceManager = theResourceManager;

            // TODO: This is weird, mendatory??
            this.gameResourceManager.AddDrawingFont(@"Sandbox\SpriteFont1");
            this.CreateScene();
        }

        public override void Update(InputContext inputContext, IGameTiming gameTime)
        {
            this.inputConfiguration.Update(inputContext, gameTime);

            Func<Samples, Color> colorFunc = sample => 
                this.hoveringSample == sample ? Color.Blue : this.currentSample == sample ? Color.Red : Color.White;

            this.sandboxRectangle.Color = colorFunc(Samples.Sandbox);
            this.shootEmUpRectangle.Color = colorFunc(Samples.ShootEmUp);
            this.tiledRectangle.Color = colorFunc(Samples.Tiled);
            this.touchRectangle.Color = colorFunc(Samples.Touch);
        }

        public override int Draw(DrawContext drawContext)
        {
            drawContext.Camera = this.camera;
            return this.scene.Draw(drawContext);
        }

        private void CreateInputConfiguration()
        {
            this.inputConfiguration = new InputConfiguration();

            // Keyboard
            this.inputConfiguration.AddDigitalButton("Back").Assign(KeyboardKeys.Escape)
                .MapClickTo(gt => this.screenNavigation.Exit());

            this.inputConfiguration.AddDigitalButton("GotoSandbox").Assign(KeyboardKeys.D1)
                .MapClickTo(gt => this.LaunchSandboxSample());

            this.inputConfiguration.AddDigitalButton("GotoShootEmUp").Assign(KeyboardKeys.D2)
                .MapClickTo(gt => this.LaunchShootEmUpSample());

            this.inputConfiguration.AddDigitalButton("GotoTiled").Assign(KeyboardKeys.D3)
                .MapClickTo(gt => this.LaunchTiledSample());

            this.inputConfiguration.AddDigitalButton("GotoTouch").Assign(KeyboardKeys.D4)
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

            this.inputConfiguration.AddMouseTracking(this.camera).OnMove((mt, e) =>
            {
                this.mouseState = mt;

                this.mouseLayer.Update(this.mouseState);

                var hit = this.scene.GetHits(this.mouseState.AbsolutePosition, this.camera).OfType<RectangleHit>().FirstOrDefault();
                this.hoveringSample = hitToSampleFunc(hit);
            });

            this.inputConfiguration.AddDigitalButton("MouseSelection").Assign(MouseButtons.Left).MapClickTo(elapse =>
            {
                var hit = this.scene.GetHits(this.mouseState.AbsolutePosition, this.camera).OfType<RectangleHit>().FirstOrDefault();
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
            this.inputConfiguration.AddTouchTracking(this.camera).OnTouch((ts, gt) =>
            {
                this.touchState = ts;

                var hit = this.touchState.Touches
                    .SelectMany(t => this.scene.GetHits(t.Position, this.camera))
                    .OfType<RectangleHit>().FirstOrDefault();

                this.hoveringSample = hitToSampleFunc(hit);
            });

            this.inputConfiguration.AddEvent("TouchSelection").Assign(TouchGestureType.Tap).MapTo(gt =>
            {
                var hit = this.scene.GetHits(this.touchState.CurrentGesture.Position, this.camera)
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
        }

        private void CreateScene()
        {
            this.scene = new Scene("HubScene");

            var font = this.gameResourceManager.GetDrawingFont(@"Sandbox\SpriteFont1");

            var hubMap = new DrawingLayer("HubMap", this.gameResourceManager);

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

            this.scene.Add(hubMap);

            this.mouseLayer = MouseCursorLayer.Create(this.gameResourceManager);
            this.scene.Add(this.mouseLayer);
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
