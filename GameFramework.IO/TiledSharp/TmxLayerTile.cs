// Distributed as part of TiledSharp, Copyright 2012 Marshall Ward
// Licensed under the Apache License, Version 2.0
// http://www.apache.org/licenses/LICENSE-2.0

namespace GameFramework.IO.TiledSharp
{
    internal class TmxLayerTile
    {
        // Tile flip bit flags
        private const uint FlippedHorizontallyFlag = 0x80000000;

        private const uint FlippedVerticallyFlag = 0x40000000;

        private const uint FlippedDiagonallyFlag = 0x20000000;

        internal TmxLayerTile(uint id, int x, int y)
        {
            var rawGid = id;
            this.X = x;
            this.Y = y;

            // Scan for tile flip bit flags
            bool flip;

            flip = (rawGid & FlippedHorizontallyFlag) != 0;
            this.HorizontalFlip = flip;

            flip = (rawGid & FlippedVerticallyFlag) != 0;
            this.VerticalFlip = flip;

            flip = (rawGid & FlippedDiagonallyFlag) != 0;
            this.DiagonalFlip = flip;

            // Zero the bit flags
            rawGid &= ~(FlippedHorizontallyFlag |
                        FlippedVerticallyFlag |
                        FlippedDiagonallyFlag);

            // Save GID remainder to int
            this.Gid = (int)rawGid;
        }

        public int Gid { get; private set; }

        public int X { get; private set; }

        public int Y { get; private set; }

        public bool HorizontalFlip { get; private set; }

        public bool VerticalFlip { get; private set; }

        public bool DiagonalFlip { get; private set; }
    }
}