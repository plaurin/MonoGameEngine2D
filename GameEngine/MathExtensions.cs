using System;
using Microsoft.Xna.Framework;

namespace WindowsGame1
{
    public static class MathExtensions
    {
        public static float Square(this float val)
        {
            return (float)Math.Pow(val, 2);
        }

        public static float SquareRoot(this float val)
        {
            return (float)Math.Pow(val, 0.5);
        }

        public static Vector2 RoundTo(this Vector2 vector, int digits)
        {
            return new Vector2((float)Math.Round(vector.X, digits), (float)Math.Round(vector.Y, digits));
        }

        public static Vector2 TranslatePolar(this Vector2 vector, float radAngle, float distance)
        {
            return vector + new Vector2((float)Math.Cos(radAngle) * distance, (float)Math.Sin(radAngle) * distance);
        }

        public static Vector2 Translate(this Vector2 vector, Point delta)
        {
            return new Vector2(vector.X + delta.X, vector.Y + delta.Y);
        }

        public static Vector2 Scale(this Vector2 vector, float zoomFactor)
        {
            return new Vector2(vector.X * zoomFactor, vector.Y * zoomFactor);
        }

        public static Point Translate(this Point point, Point delta)
        {
            return new Point(point.X + delta.X, point.Y + delta.Y);
        }

        public static Point Translate(this Point point, int deltaX, int deltaY)
        {
            return new Point(point.X + deltaX, point.Y + deltaY);
        }

        public static Point ToPoint(this Vector2 vector)
        {
            return new Point((int)vector.X, (int)vector.Y);
        }

        public static Vector2 ToVector(this Point point)
        {
            return new Vector2(point.X, point.Y);
        }

        public static Rectangle Scale(this Rectangle rectangle, float factor)
        {
            return new Rectangle(
                (int)(rectangle.X * factor),
                (int)(rectangle.Y * factor),
                (int)(rectangle.Width * factor),
                (int)(rectangle.Height * factor));
        }

        public static Rectangle Translate(this Rectangle rectangle, Point vector)
        {
            return new Rectangle(rectangle.X + vector.X, rectangle.Y + vector.Y, rectangle.Width, rectangle.Height);
        }
    }
}
