using System;
using System.Collections.Generic;
using System.Linq;

namespace ClassLibrary
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

        public static Rectangle ParseRectangle(string value)
        {
            var nameValues = GetNameValues(value);

            return new Rectangle(nameValues["X"], nameValues["Y"], nameValues["Width"], nameValues["Height"]);
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
    }
}