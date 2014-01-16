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

        private int tapCount;
        private int holdCount;
        private int doubleTapCount;
        private int dragCompleteCount;
        private int flickCount;
        private int freeDragCount;
        private int horizontalDragCount;
        private int pinchCount;
        private int pinchCompleteCount;
        private int verticalDragCount;

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

            this.inputConfiguration.AddTouchTracking(this.camera).OnTouch((ts, gt) => this.touchState = ts);

            this.inputConfiguration.EnableGesture(TouchGesture.Tap, TouchGesture.Hold, TouchGesture.DoubleTap,
                TouchGesture.DragComplete, TouchGesture.Flick, TouchGesture.FreeDrag, TouchGesture.HorizontalDrag,
                TouchGesture.Pinch, TouchGesture.PinchComplete, TouchGesture.VerticalDrag);

            this.inputConfiguration.AddEvent("Tap").Assign(TouchGesture.Tap).MapTo(gt => this.tapCount++);
            this.inputConfiguration.AddEvent("Hold").Assign(TouchGesture.Hold).MapTo(gt => this.holdCount++);
            this.inputConfiguration.AddEvent("DoubleTap").Assign(TouchGesture.DoubleTap).MapTo(gt => this.doubleTapCount++);
            this.inputConfiguration.AddEvent("DragComplete").Assign(TouchGesture.DragComplete).MapTo(gt => this.dragCompleteCount++);
            this.inputConfiguration.AddEvent("Flick").Assign(TouchGesture.Flick).MapTo(gt => this.flickCount++);
            this.inputConfiguration.AddEvent("FreeDrag").Assign(TouchGesture.FreeDrag).MapTo(gt => this.freeDragCount++);
            this.inputConfiguration.AddEvent("HorizontalDrag").Assign(TouchGesture.HorizontalDrag).MapTo(gt => this.horizontalDragCount++);
            this.inputConfiguration.AddEvent("Pinch").Assign(TouchGesture.Pinch).MapTo(gt => this.pinchCount++);
            this.inputConfiguration.AddEvent("PinchComplete").Assign(TouchGesture.PinchComplete).MapTo(gt => this.pinchCompleteCount++);
            this.inputConfiguration.AddEvent("VerticalDrag").Assign(TouchGesture.VerticalDrag).MapTo(gt => this.verticalDragCount++);

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
            this.diagnosticMap.UpdateLine("DoubleTap", this.doubleTapCount);
            this.diagnosticMap.UpdateLine("DragC", this.dragCompleteCount);
            this.diagnosticMap.UpdateLine("FreeD", this.freeDragCount);
            this.diagnosticMap.UpdateLine("Hold", this.holdCount);
            this.diagnosticMap.UpdateLine("HDrag", this.horizontalDragCount);
            this.diagnosticMap.UpdateLine("PinchC", this.pinchCompleteCount);
            this.diagnosticMap.UpdateLine("Pinch", this.pinchCount);
            this.diagnosticMap.UpdateLine("Tap", this.tapCount);
            this.diagnosticMap.UpdateLine("VDrag", this.verticalDragCount);
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

            this.diagnosticMap.AddLine("DoubleTap", "DoubleTap: {0}");
            this.diagnosticMap.AddLine("DragC", "DragComplete: {0}");
            this.diagnosticMap.AddLine("FreeD", "FreeDrag: {0}");
            this.diagnosticMap.AddLine("Hold", "Hold: {0}");
            this.diagnosticMap.AddLine("HDrag", "HorizontalDrag: {0}");
            this.diagnosticMap.AddLine("PinchC", "PinchCompleted: {0}");
            this.diagnosticMap.AddLine("Pinch", "Pinch: {0}");
            this.diagnosticMap.AddLine("Tap", "Tap: {0}");
            this.diagnosticMap.AddLine("VDrag", "VerticalDrag: {0}");

            return this.diagnosticMap;
        }
    }
}
