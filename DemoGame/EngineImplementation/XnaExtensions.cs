using System;

using ClassLibrary;

using Microsoft.Xna.Framework;

using Color = Microsoft.Xna.Framework.Color;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace WindowsGame1.EngineImplementation
{
    public static class XnaExtensions
    {
        public static Vector ToVector(this Vector2 vector2)
        {
            return new Vector(vector2.X, vector2.Y);
        }

        public static Vector2 ToVector2(this Vector vector)
        {
            return new Vector2((float)vector.X, (float)vector.Y);
        }

        public static Color ToXnaColor(this ClassLibrary.Color color)
        {
            return new Color(color.R, color.G, color.B, color.A);
        }

        public static Rectangle ToXnaRect(this ClassLibrary.Rectangle rectangle)
        {
            return new Rectangle(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
        }
    }
}