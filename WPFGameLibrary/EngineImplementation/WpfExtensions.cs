using System;

using ClassLibrary;

namespace WPFGameLibrary.EngineImplementation
{
    public static class WpfExtensions
    {
        public static System.Windows.Media.Color ToWinColor(this Color color)
        {
            return System.Windows.Media.Color.FromArgb((byte)color.A, (byte)color.R, (byte)color.G, (byte)color.B);
        }

        public static System.Windows.Rect ToRect(this Rectangle rectangle)
        {
            return new System.Windows.Rect(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
        }

        public static System.Windows.Point ToWinPoint(this Vector vector)
        {
            return new System.Windows.Point((int)vector.X, (int)vector.Y);
        }

        public static Point ToLibPoint(this System.Windows.Point point)
        {
            return new Point((int)point.X, (int)point.Y);
        }
    }
}