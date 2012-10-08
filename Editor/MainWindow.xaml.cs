using System;
using System.Windows;
using System.Windows.Input;

using DemoGameDomain;

using GameFramework;
using GameFramework.Cameras;

using WpfGameFramework;

namespace WpfGameLoader
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private readonly GameResourceManager gameResourceManager;

        private Viewport viewport;

        private WPFGameBase wpfGame;

        public MainWindow()
        {
            this.InitializeComponent();

            this.gameResourceManager = ServiceLocator.GameResourceManager;
        }

        private void Button1Click(object sender, RoutedEventArgs e)
        {
            this.button1.Visibility = Visibility.Collapsed;

            this.viewport = new Viewport((int)this.Canvas.Width, (int)this.Canvas.Height);
            new Camera(this.viewport);

            this.wpfGame = new WPFGameBase(this.viewport, this.gameResourceManager, new DefaultScreen(), 
                x => this.Canvas.Source = x);

            this.wpfGame.Start();
        }

        private void WindowPreviewKey(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (this.wpfGame != null)
                this.wpfGame.WindowPreviewKey(sender, e);
        }

        private void WindowPreviewMouseButton(object sender, MouseButtonEventArgs e)
        {
            if (this.wpfGame != null) this.wpfGame.WindowPreviewMouseButton(sender, e);
        }

        private void WindowPreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (this.wpfGame != null) this.wpfGame.WindowPreviewMouseMove(sender, e, this.Canvas);
        }
    }
}
