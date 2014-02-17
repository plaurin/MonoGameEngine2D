using System.Collections.Generic;
using GameFramework.Tiles;

namespace GameFramework.IO
{
    public interface ITiledFile
    {
        IEnumerable<TileLayer> TileLayers { get; }

        IEnumerable<ObjectLayer> ObjectLayers { get; }
    }
}