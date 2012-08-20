using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;

using ClassLibrary;

namespace Editor
{
    public class WinTexture : Texture
    {
        private BitmapSource bitmapSource;

        private List<Tuple<Rectangle, BitmapSource>> tiles;

        public WinTexture(string name, string filePath)
        {
            this.FilePath = filePath;
            this.Name = name;

            this.tiles = new List<Tuple<Rectangle, BitmapSource>>();
        }

        public string FilePath { get; private set; }

        public BitmapSource BitmapSource
        {
            get
            {
                if (this.bitmapSource == null)
                {
                    var bi = new BitmapImage();
                    // BitmapImage.UriSource must be in a BeginInit/EndInit block.
                    bi.BeginInit();
                    bi.UriSource = new Uri(this.FilePath, UriKind.RelativeOrAbsolute);
                    bi.EndInit();
                    this.bitmapSource = bi;
                }

                return this.bitmapSource;
            }
        }

        public BitmapSource GetTile(Rectangle rectangle)
        {
            var cached = this.tiles.FirstOrDefault(x => x.Item1 == rectangle);

            if (cached == null)
            {
                var tile = CreateTile(this.BitmapSource, rectangle);
                this.tiles.Add(new Tuple<Rectangle, BitmapSource>(rectangle, tile));
                return tile;
            }
            
            return cached.Item2;
        }

        private static BitmapSource CreateTile(BitmapSource source, Rectangle sourceRect)
        {
            // Calculate stride of source
            var stride = source.PixelWidth * (source.Format.BitsPerPixel / 8);

            // Create data array to hold source pixel data
            var data = new byte[stride * source.PixelHeight];

            // Copy source image pixels to the data array
            source.CopyPixels(new Int32Rect(sourceRect.X, sourceRect.Y, sourceRect.Width, sourceRect.Height), data, stride, 0);

            // Create WriteableBitmap to copy the pixel data to.
            var target = new WriteableBitmap(sourceRect.Width, sourceRect.Height, source.DpiX, source.DpiY, source.Format, source.Palette);

            // Write the pixel data to the WriteableBitmap.
            target.WritePixels(new Int32Rect(0, 0, sourceRect.Width, sourceRect.Height), data, stride, 0);

            return target;
        }
    }
}