﻿using System;
using System.Collections.Generic;

namespace GameFramework
{
    public static class MathExtensions
    {
        public static float Square(this float val)
        {
            return (float)Math.Pow(val, 2);
        }

        public static double Square(this double val)
        {
            return Math.Pow(val, 2);
        }

        public static float SquareRoot(this float val)
        {
            return (float)Math.Pow(val, 0.5);
        }

        public static double SquareRoot(this double val)
        {
            return Math.Pow(val, 0.5);
        }

        public static Vector RoundTo(this Vector vector, int digits)
        {
            return new Vector((float)Math.Round(vector.X, digits), (float)Math.Round(vector.Y, digits));
        }

        public static Vector TranslatePolar(this Vector vector, float radAngle, float distance)
        {
            return vector + new Vector((float)Math.Cos(radAngle) * distance, (float)Math.Sin(radAngle) * distance);
        }

        public static Vector Translate(this Vector vector, Point delta)
        {
            return new Vector(vector.X + delta.X, vector.Y + delta.Y);
        }

        public static Vector Translate(this Vector vector, float offsetX, float offsetY)
        {
            return new Vector(vector.X + offsetX, vector.Y + offsetY);
        }

        public static Vector Scale(this Vector vector, float zoomFactor)
        {
            return new Vector(vector.X * zoomFactor, vector.Y * zoomFactor);
        }

        public static Vector Scale(this Vector vector, Vector scalingVector)
        {
            return new Vector(vector.X * scalingVector.X, vector.Y * scalingVector.Y);
        }

        public static Point Translate(this Point point, Point delta)
        {
            return new Point(point.X + delta.X, point.Y + delta.Y);
        }

        public static Point Translate(this Point point, int deltaX, int deltaY)
        {
            return new Point(point.X + deltaX, point.Y + deltaY);
        }

        public static Point Scale(this Point point, Vector scalingVector)
        {
            return new Point((int)(point.X * scalingVector.X), (int)(point.Y * scalingVector.Y));
        }

        public static Point Scale(this Point point, float zoomFactor)
        {
            return new Point((int)(point.X * zoomFactor), (int)(point.Y * zoomFactor));
        }

        public static Point ToPoint(this Vector vector)
        {
            return new Point((int)vector.X, (int)vector.Y);
        }

        public static Vector ToVector(this Point point)
        {
            return new Vector(point.X, point.Y);
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

        public static bool Intercept(this Rectangle rectangle, Point point)
        {
            return point.X >= rectangle.X
                && point.Y >= rectangle.Y
                && point.X < rectangle.X + rectangle.Width
                && point.Y < rectangle.Y + rectangle.Height;
        }

        public static Color ChangeAlpha(this Color color, int alpha)
        {
            return new Color(color.R, color.G, color.B, alpha);
        }

        public static void ForEachPair<T>(this IEnumerable<T> enumerable, Action<T, T> action)
        {
            var first = default(T);
            var last = default(T);
            foreach (var item in enumerable)
            {
                if (object.Equals(first, default(T))) 
                    first = item;
                else 
                    action(last, item);

                last = item;
            }

            action(last, first);
        }
    }
}
