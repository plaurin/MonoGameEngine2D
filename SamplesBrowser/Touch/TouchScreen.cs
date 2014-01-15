using System;
using System.Diagnostics;
using System.Linq;
using GameFramework;
using GameFramework.Cameras;
using GameFramework.Drawing;
using GameFramework.Inputs;
using GameFramework.Scenes;
using GameFramework.Screens;
using GameFramework.Utilities;

namespace SamplesBrowser.Touch
{
    public class TouchScreen : ScreenBase
    {
        private readonly ScreenNavigation screenNavigation;
        private Camera camera;
        private InputConfiguration inputConfiguration;
        private GameResourceManager gameResourceManager;
        private Scene scene;
        private DiagnosticMap diagnosticMap;

        public TouchScreen(ScreenNavigation screenNavigation)
        {
            this.screenNavigation = screenNavigation;
        }

        public override void Initialize(Camera theCamera)
        {
            this.camera = theCamera;

            theCamera.Center = CameraCenter.WindowTopLeft;
        }

        public override InputConfiguration GetInputConfiguration()
        {
            this.inputConfiguration = new InputConfiguration();

            this.inputConfiguration.AddDigitalButton("Back").Assign(KeyboardKeys.Escape)
                .MapClickTo(gt => this.screenNavigation.NavigateBack());

            this.inputConfiguration.AddTouchTracking(this.camera).OnTouch((ts, gt) =>
            {
                if (ts.Touches.Any())
                {
                    Debugger.Break();
                }

                var toto = string.Join(", ", ts.Touches);
                var a = toto;
            });

            return this.inputConfiguration;
        }

        public override void LoadContent(GameResourceManager theResourceManager)
        {
            this.gameResourceManager = theResourceManager;
        }

        public override void Update(IGameTiming gameTime)
        {
            this.diagnosticMap.Update(gameTime, this.camera);
            this.diagnosticMap.UpdateLine("Range", "133");
        }

        public override Scene GetScene()
        {
            this.scene = new Scene("Touch");

            this.scene.AddMap(this.CreateDiagnosticMap());

            return this.scene;
        }

        private DiagnosticMap CreateDiagnosticMap()
        {
            var font = this.gameResourceManager.GetDrawingFont(@"Sandbox\SpriteFont1");

            this.diagnosticMap = new DiagnosticMap(this.gameResourceManager, font, 
                DiagnosticMapConfiguration.CreateWithFpsOnly(DiagnosticDisplayLocation.Left));

            this.diagnosticMap.AddLine("Range", "Range: {0:f1}");

            return this.diagnosticMap;
        }
    }
}
