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

        public static Point Translate(this Point point, Point delta)
        {
            return new Point(point.X + delta.X, point.Y + delta.Y);
        }

        public static Point ToPoint(this Vector2 vector)
        {
            return new Point((int)vector.X, (int)vector.Y);
        }
    }

    public static class MathUtil
    {
        public static float CalcHypotenuse(float sideA, float sideB)
        {
            return (sideA.Square() + sideB.Square()).SquareRoot();
        }

        public static float CalcHypotenuseSide(float hypotenuse, float sideA)
        {
            return (hypotenuse.Square() - sideA.Square()).SquareRoot();
        }
    }
}
