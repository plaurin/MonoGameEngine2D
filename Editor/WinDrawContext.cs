using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using ClassLibrary;

using Color = ClassLibrary.Color;

namespace Editor
{
    public class WinDrawContext : DrawContext
    {
        private readonly Viewport viewport;

        private readonly DrawingVisual drawingVisual;

        private readonly DrawingContext drawingContext;

        public WinDrawContext(Viewport viewport)
        {
            this.viewport = viewport;
            this.drawingVisual = new DrawingVisual();
            this.drawingContext = this.drawingVisual.RenderOpen();
        }

        public DrawingVisual Finish()
        {
            this.drawingContext.Close();
            return this.drawingVisual;
        }

        public void FillColor(Color color)
        {
            var brush = new SolidColorBrush(color.ToWinColor());
            this.drawingContext.DrawRectangle(brush, null, new Rect(0, 0, this.viewport.Width, this.viewport.Height));
        }

        public void DrawImage(Texture texture, Rectangle rectangle)
        {
            var winTexture = (WinTexture)texture;

            var bi = new BitmapImage();
            // BitmapImage.UriSource must be in a BeginInit/EndInit block.
            bi.BeginInit();
            bi.UriSource = new Uri(winTexture.FilePath, UriKind.RelativeOrAbsolute);
            bi.EndInit();

            this.drawingContext.DrawImage(bi, rectangle.ToRect());
        }
    }
}