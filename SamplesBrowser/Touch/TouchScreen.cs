using System;
using GameFramework;
using GameFramework.Cameras;
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

        private TouchStateBase touchState;

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
                this.touchState = ts;
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
            this.diagnosticMap.Update(this.touchState);
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

            var configuration = DiagnosticMapConfiguration.CreateWithFpsOnly(DiagnosticDisplayLocation.Left);
            configuration.DisplayTouchState = true;

            this.diagnosticMap = new DiagnosticMap(this.gameResourceManager, font, configuration);

            return this.diagnosticMap;
        }
    }
}
