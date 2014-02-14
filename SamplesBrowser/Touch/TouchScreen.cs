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
        private DiagnosticLayer diagnosticLayer;

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

            var viewport = this.camera.Viewport;
            //var size = new Size(viewport.Width, viewport.Height);
            var s2 = viewport.Size.Scale(0.1f);
            var backRectangle = new Rectangle(viewport.Width - s2.X * 2, viewport.Height - s2.Y * 2, s2.X, s2.Y);

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
            this.diagnosticLayer.Update(gameTime, this.camera);
            this.diagnosticLayer.Update(this.touchState);
            this.diagnosticLayer.UpdateLine("DoubleTap", this.doubleTapCount);
            this.diagnosticLayer.UpdateLine("DragC", this.dragCompleteCount);
            this.diagnosticLayer.UpdateLine("Flick", this.flickCount);
            this.diagnosticLayer.UpdateLine("FreeD", this.freeDragCount);
            this.diagnosticLayer.UpdateLine("Hold", this.holdCount);
            this.diagnosticLayer.UpdateLine("HDrag", this.horizontalDragCount);
            this.diagnosticLayer.UpdateLine("PinchC", this.pinchCompleteCount);
            this.diagnosticLayer.UpdateLine("Pinch", this.pinchCount);
            this.diagnosticLayer.UpdateLine("Tap", this.tapCount);
            this.diagnosticLayer.UpdateLine("VDrag", this.verticalDragCount);

            this.visualBackButtonElement.Color = this.isHoveringBackButton ? Color.Red : Color.Blue;
            this.isHoveringBackButton = false;
        }

        public override Scene GetScene()
        {
            this.scene = new Scene("Touch");

            this.scene.Add(this.CreateButtonLayer());

            this.scene.Add(this.CreateDiagnosticLayer());

            return this.scene;
        }

        public override int Draw(DrawContext drawContext)
        {
            return this.scene.Draw(drawContext);
        }

        public DrawingLayer CreateButtonLayer()
        {
            var font = this.gameResourceManager.GetDrawingFont(@"Sandbox\SpriteFont1");

            var drawingMap = new DrawingLayer("Button", this.gameResourceManager);

            this.visualBackButtonElement = drawingMap.AddRectangle(this.visualBackButton.Rectangle, 2, Color.Blue);
            drawingMap.AddText(font, "Back", this.visualBackButton.Rectangle.Location.Translate(10, 10), Color.White);

            return drawingMap;
        }

        private DiagnosticLayer CreateDiagnosticLayer()
        {
            var font = this.gameResourceManager.GetDrawingFont(@"Sandbox\SpriteFont1");

            var configuration = DiagnosticLayerConfiguration.CreateWithFpsOnly(DiagnosticDisplayLocation.Left);
            configuration.DisplayTouchState = true;

            this.diagnosticLayer = new DiagnosticLayer(this.gameResourceManager, font, configuration);

            this.diagnosticLayer.AddLine("DoubleTap", "DoubleTap: {0}");
            this.diagnosticLayer.AddLine("DragC", "DragComplete: {0}");
            this.diagnosticLayer.AddLine("Flick", "Flick: {0}");
            this.diagnosticLayer.AddLine("FreeD", "FreeDrag: {0}");
            this.diagnosticLayer.AddLine("Hold", "Hold: {0}");
            this.diagnosticLayer.AddLine("HDrag", "HorizontalDrag: {0}");
            this.diagnosticLayer.AddLine("PinchC", "PinchCompleted: {0}");
            this.diagnosticLayer.AddLine("Pinch", "Pinch: {0}");
            this.diagnosticLayer.AddLine("Tap", "Tap: {0}");
            this.diagnosticLayer.AddLine("VDrag", "VerticalDrag: {0}");

            return this.diagnosticLayer;
        }
    }
}
