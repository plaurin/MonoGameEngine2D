using System;

namespace GameFramework
{
    public struct Vector
    {
        private readonly float x;
        private readonly float y;

        public Vector(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public float X 
        { 
            get { return this.x; }
        }

        public float Y
        { 
            get { return this.y; }
        }

        public float Length
        {
            get { return (float)Math.Sqrt(Math.Pow(this.x, 2) + Math.Pow(this.y, 2)); }
        }

        public static Vector Zero
        {
            get { return new Vector(); }
        }

        public static Vector One
        {
            get { return new Vector(1, 1); }
        }

        public static Vector operator +(Vector first, Vector second)
        {
            return new Vector(first.X + second.X, first.Y + second.Y);
        }

        public static Vector operator -(Vector first, Vector second)
        {
            return new Vector(first.X - second.X, first.Y - second.Y);
        }

        public static Vector operator /(Vector vector, float factor)
        {
            return new Vector(vector.X / factor, vector.Y / factor);
        }

        public static float GetDistance(Vector first, Vector second)
        {
            return MathUtil.CalcHypotenuse(Math.Abs(first.X - second.X), Math.Abs(first.Y - second.Y));
        }

        public static float GetAngle(Vector first, Vector second)
        {
            var deltaX = second.X - first.X;
            var deltaY = second.Y - first.Y;
            return (float)Math.Atan2(deltaY, deltaX);
        }

        public static Vector operator -(Vector vector)
        {
            return new Vector(-vector.X, -vector.Y);
        }

        public override string ToString()
        {
            return string.Format("{{ X:{0:f2} Y:{1:f2} }}", this.X, this.Y);
        }
    }
}