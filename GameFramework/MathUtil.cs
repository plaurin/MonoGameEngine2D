using System;
using System.Collections.Generic;
using System.Linq;

namespace GameFramework
{
    public static class MathUtil
    {
        public static float CalcHypotenuse(float sideA, float sideB)
        {
            return (sideA.Square() + sideB.Square()).SquareRoot();
        }

        public static double CalcHypotenuse(double sideA, double sideB)
        {
            return (sideA.Square() + sideB.Square()).SquareRoot();
        }

        public static float CalcHypotenuseSide(float hypotenuse, float sideA)
        {
            return (hypotenuse.Square() - sideA.Square()).SquareRoot();
        }

        public static RectangleInt ParseRectangle(string value)
        {
            var nameValues = GetNameValues(value);

            return new RectangleInt(nameValues["X"], nameValues["Y"], nameValues["Width"], nameValues["Height"]);
        }

        public static Size ParseSize(string value)
        {
            var nameValues = GetNameValues(value);

            return new Size(nameValues["Width"], nameValues["Height"]);
        }

        public static Vector ParseVector(string value)
        {
            var nameValues = GetNameValuesFloat(value);

            return new Vector(nameValues["X"], nameValues["Y"]);
        }

        public static Point ParsePoint(string value)
        {
            var nameValues = GetNameValues(value);

            return new Point(nameValues["X"], nameValues["Y"]);
        }

        public static Color ParseColor(string value)
        {
            var nameValues = GetNameValues(value);

            return new Color(nameValues["R"], nameValues["G"], nameValues["B"], nameValues["A"]);
        }

        public static float ToRadians(float degrees)
        {
            return (float)(degrees / 180.0 * Math.PI);
        }

        public static bool IsHitPolygone(IEnumerable<Vector> polygone, Vector vector)
        {
            var isHit = false;

            polygone.ForEachPair((first, second) =>
            {
                if (((first.Y > vector.Y) != (second.Y > vector.Y))
                    && (vector.X < (second.X - first.X) * (vector.Y - first.Y)
                        / (second.Y - first.Y) + first.X))
                    isHit = !isHit;
            });

            return isHit;
        }

        public static int HexDistance(Point first, Point second)
        {
            var deltaX = second.X - first.X;
            var deltaY = second.Y - first.Y;

            var adj = first.X % 2 == 0 ? 0.5 : -0.5;
            var adx = Math.Abs(deltaX);
            var ady = Math.Abs(deltaY + (adx % 2 * adj));

            if (adx >= 2 * ady) return adx;
            return (int)(adx + ady - adx / 2.0);
        }

        public static IEnumerable<Point> AdjacentHex(Point hexPosition)
        {
            if (hexPosition.X % 2 == 0)
            {
                yield return new Point(hexPosition.X, hexPosition.Y - 1);
                yield return new Point(hexPosition.X + 1, hexPosition.Y - 1);
                yield return new Point(hexPosition.X + 1, hexPosition.Y);
                yield return new Point(hexPosition.X, hexPosition.Y + 1);
                yield return new Point(hexPosition.X - 1, hexPosition.Y);
                yield return new Point(hexPosition.X - 1, hexPosition.Y - 1);
            }
            else
            {
                yield return new Point(hexPosition.X, hexPosition.Y - 1);
                yield return new Point(hexPosition.X + 1, hexPosition.Y);
                yield return new Point(hexPosition.X + 1, hexPosition.Y + 1);
                yield return new Point(hexPosition.X, hexPosition.Y + 1);
                yield return new Point(hexPosition.X - 1, hexPosition.Y + 1);
                yield return new Point(hexPosition.X - 1, hexPosition.Y);
            }
        }

        private static Dictionary<string, float> GetNameValuesFloat(string value)
        {
            return value
                .Trim(new[] { '{', '}' })
                .Split(' ')
                .Select(x => x.Split(':'))
                .ToDictionary(x => x[0], x => float.Parse(x[1]));
        }

        private static Dictionary<string, int> GetNameValues(string value)
        {
            return value
                .Trim(new[] { '{', '}' })
                .Split(' ')
                .Select(x => x.Split(':'))
                .ToDictionary(x => x[0], x => int.Parse(x[1]));
        }

        public static Vector Clamp(Vector vector, Rectangle clampRectangle)
        {
            var x = MathUtil.Clamp(vector.X, clampRectangle.Left, clampRectangle.Right);
            var y = MathUtil.Clamp(vector.Y, clampRectangle.Top, clampRectangle.Bottom);
            return new Vector(x, y);
        }

        private static float Clamp(float value, float minValue, float maxValue)
        {
            return value < minValue
                ? minValue
                : value > maxValue
                    ? maxValue
                    : value;
        }
    }
}