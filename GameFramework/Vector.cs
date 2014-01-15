using System;

namespace GameFramework
{
    public struct Vector
    {
        private readonly double x;
        private readonly double y;

        public Vector(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public Vector(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        public double X 
        { 
            get { return this.x; }
        }

        public double Y
        { 
            get { return this.y; }
        }

        public static Vector Zero
        {
            get { return new Vector(); }
        }

        public static Vector One
        {
            get { return new Vector(1.0, 1.0); }
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

        public static double Distance(Vector first, Vector second)
        {
            return MathUtil.CalcHypotenuse(Math.Abs(first.X - second.X), Math.Abs(first.Y - second.Y));
        }

        public override string ToString()
        {
            return string.Format("{{ X:{0:f2} Y:{1:f2} }}", this.X, this.Y);
        }
    }
}