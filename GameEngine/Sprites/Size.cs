namespace WindowsGame1.Sprites
{
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
    }
}