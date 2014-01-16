using System;

namespace GameFramework
{
    /// <summary>
    /// <remarks>Should only be used for MapSize (and array size)</remarks>
    /// </summary>
    public struct Size
    {
        public int Width;
        public int Height;

        public Size(int width, int height)
        {
            this.Width = width;
            this.Height = height;
        }

        public static Size Zero
        {
            get { return new Size(0, 0); }
        }

        public override string ToString()
        {
            return string.Format("{{Width:{0} Height:{1}}}", this.Width, this.Height);
        }
    }
}
