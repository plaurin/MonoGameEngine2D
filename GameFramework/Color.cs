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

        public override string ToString()
        {
            return string.Format("{{R:{0} G:{1} B:{2} A:{3}}}", this.R, this.G, this.B, this.A);
        }
    }
}