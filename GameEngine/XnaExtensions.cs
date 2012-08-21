using System;

using ClassLibrary;

using Xna = Microsoft.Xna.Framework;

namespace WindowsGame1
{
    public static class XnaExtensions
    {
        public static Vector ToVector(this Xna.Vector2 vector2)
        {
            return new Vector(vector2.X, vector2.Y);
        }

        public static Xna.Vector2 ToVector2(this Vector vector)
        {
            return new Xna.Vector2((float)vector.X, (float)vector.Y);
        }

        public static Xna.Color ToXnaColor(this Color color)
        {
            return new Xna.Color(color.R, color.G, color.B, color.A);
        }

        public static Xna.Rectangle ToXnaRect(this Rectangle rectangle)
        {
            return new Xna.Rectangle(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
        }
    }
}