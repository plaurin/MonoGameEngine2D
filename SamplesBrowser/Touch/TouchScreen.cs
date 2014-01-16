using System;
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

        private VisualButton visualBackButton;
        private RectangleElement visualBackButtonElement;
        private bool isHoveringBackButton;

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

            this.inputConfiguration.EnableGesture(TouchGestureType.Tap, TouchGestureType.Hold, TouchGestureType.DoubleTap,
                TouchGestureType.DragComplete, TouchGestureType.Flick, TouchGestureType.FreeDrag, TouchGestureType.HorizontalDrag,
                TouchGestureType.Pinch, TouchGestureType.PinchComplete, TouchGestureType.VerticalDrag);

            this.inputConfiguration.AddEvent("Tap").Assign(TouchGestureType.Tap).MapTo(gt => this.tapCount++);
            this.inputConfiguration.AddEvent("Hold").Assign(TouchGestureType.Hold).MapTo(gt => this.holdCount++);
            this.inputConfiguration.AddEvent("DoubleTap").Assign(TouchGestureType.DoubleTap).MapTo(gt => this.doubleTapCount++);
            this.inputConfiguration.AddEvent("DragComplete").Assign(TouchGestureType.DragComplete).MapTo(gt => this.dragCompleteCount++);
            this.inputConfiguration.AddEvent("Flick").Assign(TouchGestureType.Flick).MapTo(gt => this.flickCount++);
            this.inputConfiguration.AddEvent("FreeDrag").Assign(TouchGestureType.FreeDrag).MapTo(gt => this.freeDragCount++);
            this.inputConfiguration.AddEvent("HorizontalDrag").Assign(TouchGestureType.HorizontalDrag).MapTo(gt => this.horizontalDragCount++);
            this.inputConfiguration.AddEvent("Pinch").Assign(TouchGestureType.Pinch).MapTo(gt => this.pinchCount++);
            this.inputConfiguration.AddEvent("PinchComplete").Assign(TouchGestureType.PinchComplete).MapTo(gt => this.pinchCompleteCount++);
            this.inputConfiguration.AddEvent("VerticalDrag").Assign(TouchGestureType.VerticalDrag).MapTo(gt => this.verticalDragCount++);

            var size = new Size(this.camera.Viewport.Width, this.camera.Viewport.Height);
            var s2 = size.Scale(0.1f);
            var backRectangle = new Rectangle(size.Width - s2.Width * 2, size.Height - s2.Height * 2, s2);

            this.visualBackButton = this.inputConfiguration.AddVisualButton("Back", backRectangle)
                .Assign(TouchGestureType.Tap)
                .MapTouchTo(gt => this.isHoveringBackButton = true)
                .MapClickTo(gt => this.screenNavigation.NavigateBack());

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
            this.diagnosticMap.UpdateLine("Flick", this.flickCount);
            this.diagnosticMap.UpdateLine("FreeD", this.freeDragCount);
            this.diagnosticMap.UpdateLine("Hold", this.holdCount);
            this.diagnosticMap.UpdateLine("HDrag", this.horizontalDragCount);
            this.diagnosticMap.UpdateLine("PinchC", this.pinchCompleteCount);
            this.diagnosticMap.UpdateLine("Pinch", this.pinchCount);
            this.diagnosticMap.UpdateLine("Tap", this.tapCount);
            this.diagnosticMap.UpdateLine("VDrag", this.verticalDragCount);

            this.visualBackButtonElement.Color = this.isHoveringBackButton ? Color.Red : Color.Blue;
            this.isHoveringBackButton = false;
        }

        public override Scene GetScene()
        {
            this.scene = new Scene("Touch");

            this.scene.AddMap(this.CreateButtonMap());

            this.scene.AddMap(this.CreateDiagnosticMap());

            return this.scene;
        }

        public DrawingMap CreateButtonMap()
        {
            var font = this.gameResourceManager.GetDrawingFont(@"Sandbox\SpriteFont1");

            var drawingMap = new DrawingMap("Button", this.gameResourceManager);

            this.visualBackButtonElement = drawingMap.AddRectangle(this.visualBackButton.Rectangle, 2, Color.Blue);
            drawingMap.AddText(font, "Back", this.visualBackButton.Rectangle.Location.Translate(10, 10).ToVector(), Color.White);

            return drawingMap;
        }

        private DiagnosticMap CreateDiagnosticMap()
        {
            var font = this.gameResourceManager.GetDrawingFont(@"Sandbox\SpriteFont1");

            var configuration = DiagnosticMapConfiguration.CreateWithFpsOnly(DiagnosticDisplayLocation.Left);
            configuration.DisplayTouchState = true;

            this.diagnosticMap = new DiagnosticMap(this.gameResourceManager, font, configuration);

            this.diagnosticMap.AddLine("DoubleTap", "DoubleTap: {0}");
            this.diagnosticMap.AddLine("DragC", "DragComplete: {0}");
            this.diagnosticMap.AddLine("Flick", "Flick: {0}");
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
