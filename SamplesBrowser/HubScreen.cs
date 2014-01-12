using System;
using System.Linq;
using GameFramework;
using GameFramework.Cameras;
using GameFramework.Drawing;
using GameFramework.Inputs;
using GameFramework.Maps;
using GameFramework.Scenes;
using GameFramework.Screens;
using SamplesBrowser.Sandbox;
using SamplesBrowser.ShootEmUp;
using SamplesBrowser.Tiled;

namespace SamplesBrowser
{
    public class HubScreen : ScreenBase
    {
        private readonly ScreenNavigation screenNavigation;

        private Camera camera;
        private GameResourceManager gameResourceManager;

        private Samples hoveringSample = Samples.None;
        private Samples currentSample = Samples.None;

        private RectangleElement sandboxRectangle;
        private RectangleElement shootEmUpRectangle;
        private RectangleElement tiledRectangle;

        private MouseStateBase mouseState;

        private Scene scene;
        private MouseCursorMap mouseMap;

        public HubScreen(ScreenNavigation screenNavigation)
        {
            this.screenNavigation = screenNavigation;
        }

        private enum Samples
        {
            None,
            Sandbox,
            ShootEmUp,
            Tiled
        }

        public override void Initialize(Camera theCamera)
        {
            this.camera = theCamera;

            this.camera.Center = CameraCenter.WindowTopLeft;
        }

        public override InputConfiguration GetInputConfiguration()
        {
            var input = new InputConfiguration();

            // Keyboard
            input.AddDigitalButton("Back").Assign(KeyboardKeys.Escape)
                .MapClickTo(gt => this.screenNavigation.Exit());

            input.AddDigitalButton("GotoSandbox").Assign(KeyboardKeys.D1)
                .MapClickTo(gt => this.LaunchSandboxSample());

            input.AddDigitalButton("GotoShootEmUp").Assign(KeyboardKeys.D2)
                .MapClickTo(gt => this.LaunchShootEmUpSample());

            input.AddDigitalButton("GotoTiled").Assign(KeyboardKeys.D3)
                .MapClickTo(gt => this.LaunchTiledSample());

            // Mouse
            Func<RectangleHit, Samples> hitToSampleFunc = hit =>
            {
                if (hit != null)
                {
                    if (hit.RectangleElement == this.sandboxRectangle) return Samples.Sandbox;
                    if (hit.RectangleElement == this.shootEmUpRectangle) return Samples.ShootEmUp;
                    if (hit.RectangleElement == this.tiledRectangle) return Samples.Tiled;
                }

                return Samples.None;
            };

            input.AddMouseTracking(this.camera).OnMove((mt, e) =>
            {
                this.mouseState = mt;

                this.mouseMap.Update(this.mouseState);

                var hit = this.scene.GetHits(this.mouseState.AbsolutePosition, this.camera).OfType<RectangleHit>().FirstOrDefault();
                this.hoveringSample = hitToSampleFunc(hit);
            });

            input.AddDigitalButton("Selection").Assign(MouseButtons.Left).MapClickTo(elapse =>
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
                }
            });

            return input;
        }

        public override void LoadContent(GameResourceManager theResourceManager)
        {
            this.gameResourceManager = theResourceManager;

            // TODO: This is weird, mendatory??
            this.gameResourceManager.AddDrawingFont(@"Sandbox\SpriteFont1");
        }

        public override void Update(IGameTiming gameTime)
        {
            Func<Samples, Color> colorFunc = sample => 
                this.hoveringSample == sample ? Color.Blue : this.currentSample == sample ? Color.Red : Color.White;

            this.sandboxRectangle.Color = colorFunc(Samples.Sandbox);
            this.shootEmUpRectangle.Color = colorFunc(Samples.ShootEmUp);
            this.tiledRectangle.Color = colorFunc(Samples.Tiled);
        }

        public override Scene GetScene()
        {
            this.scene = new Scene("HubScene");

            var font = this.gameResourceManager.GetDrawingFont(@"Sandbox\SpriteFont1");

            var hubMap = new DrawingMap("HubMap", this.gameResourceManager);

            this.sandboxRectangle = hubMap.AddRectangle(10, 10, 200, 200, 1, Color.White);
            this.shootEmUpRectangle = hubMap.AddRectangle(220, 10, 200, 200, 1, Color.White);
            this.tiledRectangle = hubMap.AddRectangle(10, 230, 200, 200, 1, Color.White);
            //hubMap.AddRectangle(10, 230, 200, 200, 1, Color.White);
            //hubMap.AddRectangle(220, 230, 200, 200, 1, Color.White);

            hubMap.AddText(font, "1 - Sandbox sample", new Vector(20, 210), Color.White);
            hubMap.AddText(font, "2 - ShootEmUp sample", new Vector(220, 210), Color.White);
            hubMap.AddText(font, "3 - Tiled sample", new Vector(20, 430), Color.White);

            this.scene.AddMap(hubMap);

            this.mouseMap = MouseCursorMap.Create(this.gameResourceManager);
            this.scene.AddMap(this.mouseMap);

            return this.scene;
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
    }
}
