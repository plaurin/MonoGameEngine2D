using System;
using System.Globalization;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using ClassLibrary;
using ClassLibrary.Cameras;
using ClassLibrary.Drawing;

using Color = ClassLibrary.Color;
using Vector = ClassLibrary.Vector;

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

        public void DrawImage(Texture texture, Rectangle destination)
        {
            var winTexture = (WinTexture)texture;

            var bi = new BitmapImage();
            // BitmapImage.UriSource must be in a BeginInit/EndInit block.
            bi.BeginInit();
            bi.UriSource = new Uri(winTexture.FilePath, UriKind.RelativeOrAbsolute);
            bi.EndInit();

            this.drawingContext.DrawImage(bi, destination.ToRect());
        }

        public void DrawImage(Texture texture, Rectangle source, Rectangle destination)
        {
            var winTexture = (WinTexture)texture;

            var bi = new BitmapImage();
            // BitmapImage.UriSource must be in a BeginInit/EndInit block.
            bi.BeginInit();
            bi.UriSource = new Uri(winTexture.FilePath, UriKind.RelativeOrAbsolute);
            bi.EndInit();

            var tile = this.T(bi, source);

            this.drawingContext.DrawImage(tile, destination.ToRect());
        }

        public BitmapSource T(BitmapSource source, Rectangle sourceRect)
        {
            // Calculate stride of source
            var stride = source.PixelWidth * (source.Format.BitsPerPixel / 8);

            // Create data array to hold source pixel data
            var data = new byte[stride * source.PixelHeight];

            // Copy source image pixels to the data array
            source.CopyPixels(new Int32Rect(sourceRect.X, sourceRect.Y, sourceRect.Width, sourceRect.Height), data, stride, 0);

            // Create WriteableBitmap to copy the pixel data to.
            var target = new WriteableBitmap(sourceRect.Width, sourceRect.Height, source.DpiX, source.DpiY, source.Format, source.Palette);

            // Write the pixel data to the WriteableBitmap.
            target.WritePixels(new Int32Rect(0, 0, sourceRect.Width, sourceRect.Height), data, stride, 0);

            return target;
        }

        public override void DrawString(DrawContext drawContext, Camera camera, string finalText, Vector finalVector, float finalZoomFactor, DrawingFont drawingFont, Color color)
        {
            var brush = new SolidColorBrush(color.ToWinColor());

            // Create the initial formatted text string.
            var formattedText = new FormattedText(
                finalText,
                CultureInfo.GetCultureInfo("en-us"),
                FlowDirection.LeftToRight,
                new Typeface("Verdana"),
                10,
                brush);

            // Draw the formatted text string to the DrawingContext of the control.
            this.drawingContext.DrawText(formattedText, finalVector.ToWinPoint());
        }

        public override void DrawLine(DrawContext drawContext, Camera camera, Vector @from, Vector to, Color color)
        {
            var pen = new Pen(new SolidColorBrush(color.ToWinColor()), camera.ZoomFactor);

            this.drawingContext.DrawLine(pen, @from.ToWinPoint(), to.ToWinPoint());
        }

        public override void DrawLine(Vector vectorFrom, Vector vectorTo, float width, Color color)
        {
            var pen = new Pen(new SolidColorBrush(color.ToWinColor()), width);

            this.drawingContext.DrawLine(pen, vectorFrom.ToWinPoint(), vectorTo.ToWinPoint());
        }
    }
}