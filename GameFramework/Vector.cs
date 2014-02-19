using System;

namespace GameFramework
{
    public struct Vector
    {
        private const float Epsilon = 0.00001f;

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

        public Vector GetNormalized()
        {
            return this / this.Length;
        }

        public float GetAngle()
        {
            return (float)Math.Atan2(this.Y, this.X);
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

        public static Vector operator *(Vector vector, float factor)
        {
            return new Vector(vector.X * factor, vector.Y * factor);
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

        public bool Equals(Vector other)
        {
            return this.x - other.x < Epsilon && this.y - other.y < Epsilon;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Vector && this.Equals((Vector)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (this.x.GetHashCode() * 397) ^ this.y.GetHashCode();
            }
        }

        public override string ToString()
        {
            return string.Format("{{ X:{0:f2} Y:{1:f2} }}", this.X, this.Y);
        }
    }
}