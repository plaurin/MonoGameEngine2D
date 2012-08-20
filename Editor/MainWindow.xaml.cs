using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

using ClassLibrary;
using ClassLibrary.Cameras;
using ClassLibrary.Drawing;
using ClassLibrary.Inputs;
using ClassLibrary.Maps;
using ClassLibrary.Scenes;

namespace Editor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private readonly HashSet<Key> keys;

        private readonly GameResourceManager gameResourceManager;

        private readonly Factory factory;

        private TextElement fpsElement;

        private TextElement viewPortElement;

        private TextElement translationElement;

        private TextElement positionElement;

        private TextElement zoomingElement;

        private TextElement rangeElement;

        private float range = 1.0f;

        private InputConfiguration inputConfiguration;

        private Camera camera;

        private DateTime lastFrameTime;

        private Scene scene;

        private Viewport viewport;

        private bool isInFrame;

        private float elapseTime;

        private long frameCounter;

        private long fps;

        public MainWindow()
        {
            InitializeComponent();

            this.gameResourceManager = ServiceLocator.GameResourceManager;
            this.keys = new HashSet<Key>();
        }

        private void Button1Click(object sender, RoutedEventArgs e)
        {
            this.scene = Scene.LoadFrom(this.factory, this.gameResourceManager,
                @"C:\Users\Pascal\Dev\DotNet\GitHub\XNAGameEngine2D\TestScene.xml");

            this.viewport = new Viewport((int)this.Canvas.Width, (int)this.Canvas.Height);
            this.camera = new Camera(viewport);
            this.range = 0.25f;

            var diagnosticMap = this.scene.Maps.OfType<DrawingMap>().Single(x => x.Name == "Diagnostics");

            this.fpsElement = diagnosticMap.Elements.OfType<TextElement>().Single(x => x.Text.Contains("FPS"));
            this.viewPortElement = diagnosticMap.Elements.OfType<TextElement>().Single(x => x.Text.Contains("ViewPort"));
            this.translationElement = diagnosticMap.Elements.OfType<TextElement>().Single(x => x.Text.Contains("Translation"));
            this.positionElement = diagnosticMap.Elements.OfType<TextElement>().Single(x => x.Text.Contains("Position"));
            this.zoomingElement = diagnosticMap.Elements.OfType<TextElement>().Single(x => x.Text.Contains("Zooming"));
            this.rangeElement = diagnosticMap.Elements.OfType<TextElement>().Single(x => x.Text.Contains("Range"));

            this.InitInput();

            this.lastFrameTime = DateTime.Now;
            var dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += this.Tick;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 1);
            dispatcherTimer.Start();
        }

        private void Tick(object sender, EventArgs e)
        {
            if (!this.isInFrame)
            {
                this.isInFrame = true;

                var thisFrameTime = DateTime.Now;
                var delta = thisFrameTime.Subtract(this.lastFrameTime);

                this.Update(delta);
                this.Draw(delta);

                this.lastFrameTime = thisFrameTime;
                this.isInFrame = false;
            }
        }

        private void Update(TimeSpan elapsedGameTime)
        {
            // FPS
            this.elapseTime += (float)elapsedGameTime.TotalSeconds;
            this.frameCounter++;

            if (this.elapseTime > 1)
            {
                this.fps = this.frameCounter;
                this.frameCounter = 0;
                this.elapseTime = 0;
            }

            var inputContext = new WinInputContext(this.keys);
            this.inputConfiguration.Update(inputContext, (float)elapsedGameTime.TotalSeconds);

            var colorMap = this.scene.Maps.OfType<ColorMap>().FirstOrDefault();
            if (colorMap != null)
                colorMap.Color = new ClassLibrary.Color(255, 0, 0, (int)(255 * Math.Min(this.range, 1.0f)));

            this.fpsElement.SetParameters(this.fps);
            this.viewPortElement.SetParameters(this.camera.SceneViewPort);
            this.translationElement.SetParameters(this.camera.SceneTranslationVector);
            this.positionElement.SetParameters(this.camera.Position);
            this.zoomingElement.SetParameters(this.camera.ZoomFactor);
            this.rangeElement.SetParameters(this.range);
        }

        private void Draw(TimeSpan elapsedGameTime)
        {
            // Draw
            var drawContext = new WinDrawContext(this.viewport);
            this.scene.Draw(drawContext, this.camera);

            // Draw Camera
            drawContext.DrawLine(
                this.camera.ViewPortCenter.Translate(-10, 0).ToVector(),
                this.camera.ViewPortCenter.Translate(10, 0).ToVector(), 1.0f, new ClassLibrary.Color(255, 255, 0, 255));

            drawContext.DrawLine(
                this.camera.ViewPortCenter.Translate(0, -10).ToVector(),
                this.camera.ViewPortCenter.Translate(0, 10).ToVector(), 1.0f, new ClassLibrary.Color(255, 255, 0, 255));

            var drawingVisual = drawContext.Finish();
            var bmp = new RenderTargetBitmap(this.viewport.Width, this.viewport.Height, 96, 96, PixelFormats.Pbgra32);
            bmp.Render(drawingVisual);

            this.Canvas.Source = bmp;
        }

        private void InitInput()
        {
            this.inputConfiguration = new InputConfiguration();
            this.inputConfiguration.AddDigitalButton("Left").Assign(Keys.Left).MapTo(elapse => this.camera.Move(-60 * elapse, 0));
            this.inputConfiguration.AddDigitalButton("Right").Assign(Keys.Right).MapTo(elapse => this.camera.Move(60 * elapse, 0));
            this.inputConfiguration.AddDigitalButton("Up").Assign(Keys.Up).MapTo(elapse => this.camera.Move(0, -60 * elapse));
            this.inputConfiguration.AddDigitalButton("Down").Assign(Keys.Down).MapTo(elapse => this.camera.Move(0, 60 * elapse));
            this.inputConfiguration.AddDigitalButton("ZoomIn").Assign(Keys.A).MapTo(elapse => this.camera.ZoomFactor *= 1.2f * (1 + elapse));
            this.inputConfiguration.AddDigitalButton("ZoomOut").Assign(Keys.Z).MapTo(elapse => this.camera.ZoomFactor *= 1 / (1.2f * (1 + elapse)));
            this.inputConfiguration.AddDigitalButton("RangeUp").Assign(Keys.W).MapTo(elapse => this.range *= 1.2f * (1 + elapse));
            this.inputConfiguration.AddDigitalButton("RangeDown").Assign(Keys.Q).MapTo(elapse => this.range *= 1 / (1.2f * (1 + elapse)));
        }

        private void WindowPreviewKey(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.IsDown && !this.keys.Contains(e.Key)) this.keys.Add(e.Key);
            if (e.IsUp && this.keys.Contains(e.Key)) this.keys.Remove(e.Key);
        }
    }
}
