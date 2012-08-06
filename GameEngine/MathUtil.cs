using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;

using WindowsGame1.Sprites;

namespace WindowsGame1
{
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