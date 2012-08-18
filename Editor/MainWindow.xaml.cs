using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using ClassLibrary;
using ClassLibrary.Cameras;
using ClassLibrary.Scenes;

namespace Editor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private readonly GameResourceManager gameResourceManager;

        private readonly Factory factory;

        public MainWindow()
        {
            InitializeComponent();

            this.gameResourceManager = ServiceLocator.GameResourceManager;
            this.factory = ServiceLocator.Factory;
        }

        private void Button1Click(object sender, RoutedEventArgs e)
        {
            var scene = Scene.LoadFrom(this.factory, this.gameResourceManager,
                @"C:\Users\Pascal\Dev\DotNet\GitHub\XNAGameEngine2D\TestScene.xml");

            var viewport = new Viewport((int)this.Canvas.Width, (int)this.Canvas.Height);
            var drawContext = new WinDrawContext(viewport);
            var camera = new Camera(viewport);

            scene.Draw(drawContext, camera);

            var drawingVisual = drawContext.Finish();
            var bmp = new RenderTargetBitmap(viewport.Width, viewport.Height, 96, 96, PixelFormats.Pbgra32);
            bmp.Render(drawingVisual);

            this.Canvas.Source = bmp;
        }
    }
}
