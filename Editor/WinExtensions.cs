using System;

using ClassLibrary;

namespace Editor
{
    public static class WinExtensions
    {
        public static System.Windows.Media.Color ToWinColor(this Color color)
        {
            return System.Windows.Media.Color.FromArgb((byte)color.A, (byte)color.R, (byte)color.G, (byte)color.B);
        }

        public static System.Windows.Rect ToRect(this Rectangle rectangle)
        {
            return new System.Windows.Rect(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
        }
    }
}