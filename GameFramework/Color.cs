using System;

namespace GameFramework
{
    public struct Color
    {
        public Color(int r, int g, int b, int a)
            : this()
        {
            this.R = r;
            this.G = g;
            this.B = b;
            this.A = a;
        }

        public int R { get; private set; }

        public int G { get; private set; }

        public int B { get; private set; }

        public int A { get; private set; }

        public static Color Red
        {
            get { return new Color(255, 0, 0, 255); }
        }

        public static Color Green
        {
            get { return new Color(0, 255, 0, 255); }
        }

        public static Color Blue
        {
            get { return new Color(0, 0, 255, 255); }
        }

        public static Color White
        {
            get { return new Color(255, 255, 255, 255); }
        }

        public static Color Yellow
        {
            get { return new Color(255, 255, 0, 255); }
        }

        public Color Multiply(Color color)
        {
            return new Color(
                (int)(((this.R / 255.0f) * (color.R / 255.0f)) * 255.0f),
                (int)(((this.G / 255.0f) * (color.G / 255.0f)) * 255.0f),
                (int)(((this.B / 255.0f) * (color.B / 255.0f)) * 255.0f),
                (int)(((this.A / 255.0f) * (color.A / 255.0f)) * 255.0f));
        }

        public bool Equals(Color other)
        {
            return this.R == other.R && this.G == other.G && this.B == other.B && this.A == other.A;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Color && this.Equals((Color)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = this.R;
                hashCode = (hashCode * 397) ^ this.G;
                hashCode = (hashCode * 397) ^ this.B;
                hashCode = (hashCode * 397) ^ this.A;
                return hashCode;
            }
        }

        public override string ToString()
        {
            return string.Format("{{R:{0} G:{1} B:{2} A:{3}}}", this.R, this.G, this.B, this.A);
        }
    }
}