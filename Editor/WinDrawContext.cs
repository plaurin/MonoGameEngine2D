using System;
using System.Globalization;
using System.Windows;
using System.Windows.Media;

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

        public override void FillColor(Color color)
        {
            var brush = new SolidColorBrush(color.ToWinColor());
            this.drawingContext.DrawRectangle(brush, null, new Rect(0, 0, this.viewport.Width, this.viewport.Height));
        }

        public override void DrawImage(Texture texture, Rectangle destination)
        {
            var winTexture = (WinTexture)texture;

            this.drawingContext.DrawImage(winTexture.BitmapSource, destination.ToRect());
        }

        public override void DrawImage(Texture texture, Rectangle source, Rectangle destination)
        {
            var winTexture = (WinTexture)texture;

            var tile = winTexture.GetTile(source);

            this.drawingContext.DrawImage(tile, destination.ToRect());
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
                11,
                brush);

            // Draw the formatted text string to the DrawingContext of the control.
            this.drawingContext.DrawText(formattedText, finalVector.ToWinPoint());
        }

        public override void DrawLine(Vector vectorFrom, Vector vectorTo, float width, Color color)
        {
            var pen = new Pen(new SolidColorBrush(color.ToWinColor()), width);

            this.drawingContext.DrawLine(pen, vectorFrom.ToWinPoint(), vectorTo.ToWinPoint());
        }
    }
}