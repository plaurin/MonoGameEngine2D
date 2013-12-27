using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

using GameFramework;
using GameFramework.Cameras;
using GameFramework.Inputs;
using GameFramework.Scenes;
using GameFramework.Screens;

using WpfGameFramework.EngineImplementation;

using Color = GameFramework.Color;
using Point = System.Windows.Point;

namespace WpfGameFramework
{
    public class WPFGameBase
    {
        private readonly Viewport viewport;

        private readonly GameResourceManager gameResourceManager;

        private readonly ScreenBase screen;

        private readonly Action<ImageSource> imageSetterAction;

        private readonly HashSet<Key> keys;

        private readonly Dictionary<MouseButton, MouseButtonState> buttons;

        private InputConfiguration inputConfiguration;

        private Camera camera;

        private Scene scene;

        private Point mousePosition;

        private DateTime lastFrameTime;

        private DateTime firstFrameTime;

        private bool isInFrame;
        
        private readonly GameTimer gameTimer;

        public WPFGameBase(Viewport viewport, GameResourceManager gameResourceManager, ScreenBase initialScreen, 
            Action<ImageSource> imageSetterAction)
        {
            this.viewport = viewport;
            this.gameResourceManager = gameResourceManager;
            this.screen = initialScreen;
            this.imageSetterAction = imageSetterAction;
            this.gameTimer = new GameTimer();

            this.keys = new HashSet<Key>();
            this.buttons = new Dictionary<MouseButton, MouseButtonState>();
            this.buttons[MouseButton.Left] = MouseButtonState.Released;
            this.buttons[MouseButton.Middle] = MouseButtonState.Released;
            this.buttons[MouseButton.Right] = MouseButtonState.Released;
        }

        public void Start()
        {
            this.Initialize();
            this.LoadContent();

            this.lastFrameTime = DateTime.Now;
            this.firstFrameTime = this.lastFrameTime;
            var dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += this.Tick;
            dispatcherTimer.Interval = TimeSpan.FromMilliseconds(1);
            dispatcherTimer.Start();
        }

        public void WindowPreviewKey(object sender, KeyEventArgs e)
        {
            if (e.IsDown && !this.keys.Contains(e.Key)) this.keys.Add(e.Key);
            if (e.IsUp && this.keys.Contains(e.Key)) this.keys.Remove(e.Key);
        }

        public void WindowPreviewMouseButton(object sender, MouseButtonEventArgs e)
        {
            this.buttons[MouseButton.Left] = e.LeftButton;
            this.buttons[MouseButton.Middle] = e.MiddleButton;
            this.buttons[MouseButton.Right] = e.RightButton;
        }

        public void WindowPreviewMouseMove(object sender, MouseEventArgs e, IInputElement inputElement)
        {
            this.mousePosition = e.GetPosition(inputElement);
        }

        private void Initialize()
        {
            this.camera = new Camera(this.viewport);

            this.screen.Initialize(this.camera);

            this.inputConfiguration = this.screen.GetInputConfiguration();
        }

        private void LoadContent()
        {
            this.screen.LoadContent(this.gameResourceManager);

            this.scene = this.screen.GetScene();
        }

        private void Tick(object sender, EventArgs e)
        {
            if (!this.isInFrame)
            {
                this.isInFrame = true;

                var thisFrameTime = DateTime.Now;
                var delta = thisFrameTime.Subtract(this.lastFrameTime);
                var total = thisFrameTime.Subtract(this.firstFrameTime);

                this.UpdateFrame(delta, total);
                this.DrawFrame(delta);

                this.lastFrameTime = thisFrameTime;
                this.isInFrame = false;
            }
        }

        private void UpdateFrame(TimeSpan elapsedGameTime, TimeSpan totalGameTime)
        {
            this.gameTimer.Update(elapsedGameTime, totalGameTime);

            var inputContext = new WpfInputContext(this.keys, this.buttons, this.mousePosition.ToLibPoint());
            this.inputConfiguration.Update(inputContext, gameTimer);

            this.screen.Update(this.gameTimer);
        }

        private void DrawFrame(TimeSpan elapsedGameTime)
        {
            // Draw
            var drawContext = new WpfDrawContext(this.viewport);
            this.scene.Draw(drawContext, this.camera);

            // Draw Camera
            this.DrawCamera(drawContext);

            var drawingVisual = drawContext.Finish();
            var bmp = new RenderTargetBitmap(this.viewport.Width, this.viewport.Height, 96, 96, PixelFormats.Pbgra32);
            bmp.Render(drawingVisual);

            this.imageSetterAction.Invoke(bmp);
        }

        private void DrawCamera(DrawContext drawContext)
        {
            drawContext.DrawLine(
                this.camera.ViewPortCenter.Translate(-10, 0).ToVector(),
                this.camera.ViewPortCenter.Translate(10, 0).ToVector(), 1.0f, Color.Yellow);

            drawContext.DrawLine(
                this.camera.ViewPortCenter.Translate(0, -10).ToVector(),
                this.camera.ViewPortCenter.Translate(0, 10).ToVector(), 1.0f, Color.Yellow);
        }
    }
}
