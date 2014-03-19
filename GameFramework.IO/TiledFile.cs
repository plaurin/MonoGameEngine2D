using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GameFramework.IO.TiledSharp;
using GameFramework.Tiles;

namespace GameFramework.IO
{
    public class TiledFile : ITiledFile
    {
        private const string ContentFolder = "Content";

        private readonly List<TileLayer> tileLayers;
        private readonly List<ObjectLayer> objectLayers;

        private TiledFile(IEnumerable<TileLayer> tileLayers, IEnumerable<ObjectLayer> objectLayers)
        {
            this.tileLayers = new List<TileLayer>(tileLayers);
            this.objectLayers = new List<ObjectLayer>(objectLayers);
        }

        public IEnumerable<TileLayer> TileLayers
        {
            get { return this.tileLayers; }
        }

        public IEnumerable<ObjectLayer> ObjectLayers
        {
            get { return this.objectLayers; }
        }

        public static ITiledFile Load(string filepath, GameResourceManager gameResourceManager)
        {
            var map = new TmxMap(Path.Combine(ContentFolder, filepath));

            var mapSize = new Size(map.Width, map.Height);
            var tilesSize = new Size(map.TileWidth, map.TileHeight);

            var sheets = map.Tilesets.Select(tileset => LoadTileset(tileset, tilesSize, gameResourceManager)).ToList();

            var tileDefinitions = sheets.SelectMany(sheet => sheet.Definitions.Values)
                .ToDictionary(tileDefinition => tileDefinition.Name);

            var tileLayers = map.Layers.Select(l => LoadLayer(l, mapSize, tilesSize, tileDefinitions)).ToList();
            var objectLayers = map.ObjectGroups.Select(o => LoadObjects(o, tileDefinitions)).ToList();

            return new TiledFile(tileLayers, objectLayers);
        }

        private static TileSheet LoadTileset(TmxTileset tileset, Size tilesSize, GameResourceManager gameResourceManager)
        {
            var texturePath = Path.Combine(Path.GetDirectoryName(tileset.Image.Source), Path.GetFileNameWithoutExtension(tileset.Image.Source))
                .Remove(0, ContentFolder.Length + 1);
            //var texture = gameResourceManager.GetTexture(@"Tiled\tmw_desert_spacing");
            var texture = gameResourceManager.GetTexture(texturePath);
            //var sheet = new TileSheet(texture, "Desert", new Size(tmxMap.TileWidth, tmxMap.TileHeight));

            var sheet = gameResourceManager.GetTileSheet(tileset.Name);
            if (sheet == null)
            {
                sheet = new TileSheet(texture, tileset.Name, tilesSize);

                var margin = tileset.Margin;
                var spacing = tileset.Spacing;
                var numTileWidth = (tileset.Image.Width - (2 * margin) + spacing) / (tileset.TileWidth + spacing);
                var numTileHeight = (tileset.Image.Height - (2 * margin) + spacing) / (tileset.TileHeight + spacing);

                for (var j = 0; j < numTileHeight; j++)
                    for (var i = 0; i < numTileWidth; i++)
                        sheet.CreateTileDefinition((tileset.FirstGid + i + numTileWidth * j).ToString(),
                            new Point(margin + i * (tileset.TileWidth + spacing), margin + j * (tileset.TileHeight + spacing)));

                gameResourceManager.AddTileSheet(sheet);
            }

            return sheet;
        }

        private static TileLayer LoadLayer(TmxLayer layer, Size mapSize, Size tilesSize, IReadOnlyDictionary<string, TileDefinition> tileDefinitions)
        {
            var tileLayer = new ScalableTileLayer(layer.Name, mapSize, tilesSize);

            foreach (var tile in layer.Tiles)
            {
                if (tile.Gid != 0)
                    tileLayer[tile.X, tile.Y] = tileDefinitions[tile.Gid.ToString()];
            }

            return tileLayer;
        }

        private static ObjectLayer LoadObjects(TmxObjectGroup objectGroup, IReadOnlyDictionary<string, TileDefinition> tileDefinitions)
        {
            var tiledObjects = new List<TiledObject>();

            foreach (var tmxObject in objectGroup.Objects)
            {
                if (tmxObject.ObjectType == TmxObjectGroup.TmxObjectType.Tile && tmxObject.Tile != null)
                {
                    var tileDefinition = tileDefinitions[tmxObject.Tile.Gid.ToString()];
                    tiledObjects.Add(new TiledObject(tileDefinition, new Vector(tmxObject.Tile.X, tmxObject.Tile.Y)));
                }
            }

            return new ObjectLayer(objectGroup.Name, tiledObjects);
        }
    }
}
