using System;

using GameFramework;

using Microsoft.Xna.Framework;

using Color = Microsoft.Xna.Framework.Color;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace MonoGameImplementation.EngineImplementation
{
    public static class XnaExtensions
    {
        public static Vector ToVector(this Vector2 vector2)
        {
            return new Vector(vector2.X, vector2.Y);
        }

        public static Vector2 ToVector2(this Vector vector)
        {
            return new Vector2(vector.X, vector.Y);
        }

        public static Color ToXnaColor(this GameFramework.Color color)
        {
            return new Color(color.R, color.G, color.B, color.A);
        }

        public static Rectangle ToXnaRect(this GameFramework.Rectangle rectangle)
        {
            return new Rectangle((int)rectangle.X, (int)rectangle.Y, (int)rectangle.Width, (int)rectangle.Height);
        }

        public static Rectangle ToXnaRect(this GameFramework.RectangleInt rectangle)
        {
            return new Rectangle(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
        }
    }
}