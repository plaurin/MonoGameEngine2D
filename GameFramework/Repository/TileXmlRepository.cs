using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using GameFramework.Tiles;

namespace GameFramework.Repository
{
    public static class TileXmlRepository
    {
        public static XElement ToXml(TileMap tileMap)
        {
            var tileReferences = Enumerable.ToList(CreateTileReferences(tileMap));

            return new XElement("TileMap",
                XmlRepository.MapBaseToXml(tileMap),
                new XElement("MapSize", tileMap.MapSize),
                new XElement("TileSize", tileMap.TileSize),
                new XElement("TileDefinitionReferences", tileReferences.Select(x =>
                    new XElement("Reference",
                        new XAttribute("id", x.Id),
                        new XAttribute("sheetName", x.Definition.SheetName),
                        new XAttribute("name", x.Definition.Name)))),
                new XElement("Tiles", GetRowsXml(tileMap, tileReferences)));
        }

        private static IEnumerable<TileMap.TileReference> CreateTileReferences(TileMap tileMap)
        {
            var tileDefinitions = new List<TileDefinition>();
            for (var i = 0; i < tileMap.MapSize.Width; i++)
                for (var j = 0; j < tileMap.MapSize.Height; j++)
                {
                    tileDefinitions.Add(tileMap[i, j]);
                }

            return tileDefinitions
                .Distinct()
                .Select((x, i) => new TileMap.TileReference { Id = i, Definition = x });
        }

        private static IEnumerable<XElement> GetRowsXml(TileMap tileMap, List<TileMap.TileReference> tileReferences)
        {
            for (var i = 0; i < tileMap.MapSize.Width; i++)
            {
                var row = new int[tileMap.MapSize.Height];
                for (var j = 0; j < tileMap.MapSize.Height; j++)
                {
                    row[j] = tileReferences.Single(x => x.Definition == tileMap[i, j]).Id;
                }

                yield return new XElement("Row", String.Join(", ", row));
            }
        }

        public static TileMap TileMapFromXml(GameResourceManager gameResourceManager, XElement mapElement)
        {
            var name = mapElement.Attribute("name").Value;
            var mapSize = MathUtil.ParseSize(mapElement.Element("MapSize").Value);
            var tileSize = MathUtil.ParseSize(mapElement.Element("TileSize").Value);
            var tileReferences = Enumerable.ToList(GetTileReferences(gameResourceManager, mapElement.Element("TileDefinitionReferences")));
            var tiles = GetTileRowsFromXml(mapElement.Element("Tiles"));

            var map = new TileMap(name, mapSize, tileSize);
            //var map = factory.CreateTileMap(name, mapSize, tileSize);
            map.BaseFromXml(mapElement);

            int x = 0;
            foreach (var row in tiles)
            {
                int y = 0;
                foreach (var element in row)
                {
                    map[x, y++] = tileReferences.Single(r => r.Id == element).Definition;
                }

                x++;
            }

            return map;
        }

        private static IEnumerable<IEnumerable<int>> GetTileRowsFromXml(XElement rowsElement)
        {
            return rowsElement.Elements()
                .Select(rowElement => rowElement.Value.Split(',').Select(x => Int32.Parse(x.Trim())));
        }

        private static IEnumerable<TileMap.TileReference> GetTileReferences(GameResourceManager gameResourceManager, XElement tileReferencesElement)
        {
            return tileReferencesElement.Elements()
                .Select(x => new TileMap.TileReference
                {
                    Id = Int32.Parse(x.Attribute("id").Value),
                    Definition = gameResourceManager
                        .GetTileSheet(x.Attribute("sheetName").Value)
                        .Definitions[x.Attribute("name").Value]
                });
        }

        public static IEnumerable<object> GetXml(TileSheet tileSheet)
        {
            yield return new XElement("TileSize", tileSheet.TilesSize);
            yield return new XElement("Definitions", tileSheet.Definitions.Select(d => d.Value.GetXml()));
        }

        [Obsolete]
        public static TileSheet FromXml(XElement sheetElement, string name, Texture texture)
        {
            var tileSize = MathUtil.ParseSize(sheetElement.Element("TileSize").Value);
            var tileSheet = new TileSheet(texture, name, tileSize);

            foreach (var definitionElement in sheetElement.Elements("Definitions").Elements())
            {
                var tileDefinition = TileDefinition.FromXml(definitionElement, tileSheet);
                tileSheet.AddTileDefinition(tileDefinition);
            }

            return tileSheet;
        }
    }
}