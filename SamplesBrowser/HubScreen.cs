using System;
using GameFramework;
using GameFramework.Cameras;
using GameFramework.Drawing;
using GameFramework.Inputs;
using GameFramework.Scenes;
using GameFramework.Screens;

namespace SamplesBrowser
{
    public class HubScreen : ScreenBase
    {
        private Camera camera;
        private GameResourceManager gameResourceManager;

        private Samples currentSample = Samples.None;

        private RectangleElement sandboxRectangle;
        private RectangleElement shootEmUpRectangle;

        private enum Samples
        {
            None,
            Sandbox,
            ShootEmUp
        }

        public override void Initialize(Camera theCamera)
        {
            this.camera = theCamera;
            
            this.camera.Center = CameraCenter.WindowTopLeft;
        }

        public override InputConfiguration GetInputConfiguration()
        {
            var input = new InputConfiguration();

            input.AddDigitalButton("GotoSandbox").Assign(KeyboardKeys.D1)
                .MapClickTo(gt => { this.currentSample = Samples.Sandbox; });

            input.AddDigitalButton("GotoShootEmUp").Assign(KeyboardKeys.D2)
                .MapClickTo(gt => { this.currentSample = Samples.ShootEmUp; });

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
            this.sandboxRectangle.Color = this.currentSample == Samples.Sandbox ? Color.Red : Color.White;
            this.shootEmUpRectangle.Color = this.currentSample == Samples.ShootEmUp ? Color.Red : Color.White;
        }

        public override Scene GetScene()
        {
            var scene = new Scene("HubScene");

            var font = this.gameResourceManager.GetDrawingFont(@"Sandbox\SpriteFont1");

            var hubMap = new DrawingMap("HubMap", this.gameResourceManager);

            this.sandboxRectangle = hubMap.AddRectangle(10, 10, 200, 200, 1, Color.White);
            this.shootEmUpRectangle = hubMap.AddRectangle(220, 10, 200, 200, 1, Color.White);
            //hubMap.AddRectangle(10, 230, 200, 200, 1, Color.White);
            //hubMap.AddRectangle(220, 230, 200, 200, 1, Color.White);

            hubMap.AddText(font, "1 - Sandbox sample", new Vector(20, 210), Color.White);
            hubMap.AddText(font, "2 - ShootEmUp sample", new Vector(220, 210), Color.White);

            scene.AddMap(hubMap);

            return scene;
        }
    }
}
