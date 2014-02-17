using System.Collections.Generic;

namespace GameFramework.IO
{
    public class ObjectLayer
    {
        public ObjectLayer(string layerName, IEnumerable<TiledObject> tiledObjects)
        {
            this.LayerName = layerName;
            this.TiledObjects = tiledObjects;
        }

        public string LayerName { get; private set; }

        public IEnumerable<TiledObject> TiledObjects { get; private set; }
    }
}