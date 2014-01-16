using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GameFramework.IO.TiledSharp;
using GameFramework.Tiles;

namespace GameFramework.IO
{
    public static class TiledHelper
    {
        private const string ContentFolder = "Content";

        public static IEnumerable<TileLayer> LoadFile(string filepath, GameResourceManager gameResourceManager)
        {
            var map = new TmxMap(Path.Combine(ContentFolder, filepath));

            var mapSize = new Size(map.Width, map.Height);
            var tilesSize = new Size(map.TileWidth, map.TileHeight);

            var sheets = map.Tilesets.Select(tileset => LoadTileset(tileset, tilesSize, gameResourceManager)).ToList();

            var tileDefinitions = sheets.SelectMany(sheet => sheet.Definitions.Values)
                .ToDictionary(tileDefinition => tileDefinition.Name);

            foreach (var layer in map.Layers)
                yield return LoadLayer(layer, mapSize, tilesSize, tileDefinitions);
        }

        private static TileSheet LoadTileset(TmxTileset tileset, Size tilesSize, GameResourceManager gameResourceManager)
        {
            var texturePath = Path.Combine(Path.GetDirectoryName(tileset.Image.Source), Path.GetFileNameWithoutExtension(tileset.Image.Source))
                .Remove(0, ContentFolder.Length + 1);
            //var texture = gameResourceManager.GetTexture(@"Tiled\tmw_desert_spacing");
            var texture = gameResourceManager.GetTexture(texturePath);
            //var sheet = new TileSheet(texture, "Desert", new Size(tmxMap.TileWidth, tmxMap.TileHeight));
            var sheet = new TileSheet(texture, tileset.Name, tilesSize);

            var margin = tileset.Margin;
            var spacing = tileset.Spacing;
            var numTileWidth = (tileset.Image.Width - (2 * margin) + spacing) / (tileset.TileWidth + spacing);
            var numTileHeight = (tileset.Image.Height - (2 * margin) + spacing) / (tileset.TileHeight + spacing);

            for (var j = 0; j < numTileHeight; j++)
                for (var i = 0; i < numTileWidth; i++)
                    sheet.CreateTileDefinition((tileset.FirstGid + i + numTileWidth * j).ToString(),
                        new Point(margin + i * (tileset.TileWidth + spacing), margin + j * (tileset.TileHeight + spacing)));

            gameResourceManager.AddTileSheet(sheet);

            return sheet;
        }

        private static TileLayer LoadLayer(TmxLayer layer, Size mapSize, Size tilesSize, IDictionary<string, TileDefinition> tileDefinitions)
        {
            var tileLayer = new TileLayer(layer.Name, mapSize, tilesSize);

            foreach (var tile in layer.Tiles)
            {
                if (tile.Gid != 0)
                    tileLayer[tile.X, tile.Y] = tileDefinitions[tile.Gid.ToString()];
            }

            return tileLayer;
        }
    }
}
