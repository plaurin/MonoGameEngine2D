using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using GameFramework.Tiles;

namespace GameFramework.IO.Repositories
{
    public static class TileXmlRepository
    {
        public static XElement ToXml(TileLayer tileLayer)
        {
            var tileReferences = CreateTileReferences(tileLayer).ToList();

            return new XElement("TileLayer",
                XmlRepository.LayerBaseToXml(tileLayer),
                new XElement("MapSize", tileLayer.MapSize),
                new XElement("TileSize", tileLayer.TileSize),
                new XElement("TileDefinitionReferences", tileReferences.Select(x =>
                    new XElement("Reference",
                        new XAttribute("id", x.Id),
                        new XAttribute("sheetName", x.Definition.SheetName),
                        new XAttribute("name", x.Definition.Name)))),
                new XElement("Tiles", GetRowsXml(tileLayer, tileReferences)));
        }

        public static TileLayer TileLayerFromXml(GameResourceManager gameResourceManager, XElement layerElement)
        {
            var name = layerElement.Attribute("name").Value;
            var mapSize = MathUtil.ParseSize(layerElement.Element("MapSize").Value);
            var tileSize = MathUtil.ParseSize(layerElement.Element("TileSize").Value);
            var tileReferences = GetTileReferences(gameResourceManager, layerElement.Element("TileDefinitionReferences")).ToList();
            var tiles = GetTileRowsFromXml(layerElement.Element("Tiles"));

            var layer = new TileLayer(name, mapSize, tileSize);
            XmlRepository.BaseFromXml(layer, layerElement);

            int x = 0;
            foreach (var row in tiles)
            {
                int y = 0;
                foreach (var element in row)
                {
                    layer[x, y++] = tileReferences.Single(r => r.Id == element).Definition;
                }

                x++;
            }

            return layer;
        }

        public static IEnumerable<object> GetXml(TileSheet tileSheet)
        {
            yield return new XElement("TileSize", tileSheet.TilesSize);
            yield return new XElement("Definitions", tileSheet.Definitions.Select(d => GetXml(d.Value)));
        }

        public static TileSheet FromXml(XElement sheetElement, string name, Texture texture)
        {
            var tileSize = MathUtil.ParseSize(sheetElement.Element("TileSize").Value);
            var tileSheet = new TileSheet(texture, name, tileSize);

            foreach (var definitionElement in sheetElement.Elements("Definitions").Elements())
            {
                var tileDefinition = FromXml(definitionElement, tileSheet);
                tileSheet.AddTileDefinition(tileDefinition);
            }

            return tileSheet;
        }

        private static IEnumerable<TileReference> CreateTileReferences(TileLayer tileLayer)
        {
            var tileDefinitions = new List<TileDefinition>();
            for (var i = 0; i < tileLayer.MapSize.Width; i++)
                for (var j = 0; j < tileLayer.MapSize.Height; j++)
                {
                    tileDefinitions.Add(tileLayer[i, j]);
                }

            return tileDefinitions
                .Distinct()
                .Select((x, i) => new TileReference { Id = i, Definition = x });
        }

        private static IEnumerable<XElement> GetRowsXml(TileLayer tileLayer, List<TileReference> tileReferences)
        {
            for (var i = 0; i < tileLayer.MapSize.Width; i++)
            {
                var row = new int[tileLayer.MapSize.Height];
                for (var j = 0; j < tileLayer.MapSize.Height; j++)
                {
                    row[j] = tileReferences.Single(x => x.Definition == tileLayer[i, j]).Id;
                }

                yield return new XElement("Row", string.Join(", ", row));
            }
        }

        private static IEnumerable<IEnumerable<int>> GetTileRowsFromXml(XElement rowsElement)
        {
            return rowsElement.Elements()
                .Select(rowElement => rowElement.Value.Split(',').Select(x => int.Parse(x.Trim())));
        }

        private static IEnumerable<TileReference> GetTileReferences(GameResourceManager gameResourceManager, XElement tileReferencesElement)
        {
            return tileReferencesElement.Elements()
                .Select(x => new TileReference
                {
                    Id = int.Parse(x.Attribute("id").Value),
                    Definition = gameResourceManager
                        .GetTileSheet(x.Attribute("sheetName").Value)
                        .Definitions[x.Attribute("name").Value]
                });
        }

        private static XElement GetXml(TileDefinition tileDefinition)
        {
            return new XElement("TileDefinition",
                new XAttribute("name", tileDefinition.Name),
                new XAttribute("rectangle", tileDefinition.Rectangle));
        }

        private static TileDefinition FromXml(XElement definitionElement, TileSheet tileSheet)
        {
            var name = definitionElement.Attribute("name").Value;
            var rectangle = MathUtil.ParseRectangle(definitionElement.Attribute("rectangle").Value);

            return new TileDefinition(tileSheet, name, rectangle);
        }

        internal struct TileReference
        {
            public int Id { get; set; }

            public TileDefinition Definition { get; set; }
        }
    }
}