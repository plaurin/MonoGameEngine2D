using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using GameFramework.Hexes;

namespace GameFramework.IO.Repositories
{
    public class HexXmlRepository
    {
        public static XElement ToXml(HexLayer hexLayer)
        {
            var hexReferences = CreateHexReferences(hexLayer).ToList();

            return new XElement("HexLayer", XmlRepository.LayerBaseToXml(hexLayer),
                new XElement("MapSize", hexLayer.MapSize),
                new XElement("HexSize", hexLayer.HexSize),
                new XElement("EdgeLength", hexLayer.TopEdgeLength),
                new XElement("HexDefinitionReferences", hexReferences.Select(x =>
                    new XElement("Reference",
                        new XAttribute("id", x.Id),
                        new XAttribute("sheetName", x.Definition.SheetName),
                        new XAttribute("name", x.Definition.Name)))),
                new XElement("Hexes", GetRowsXml(hexLayer, hexReferences)));
        }

        public static HexLayer HexLayerFromXml(GameResourceManager gameResourceManager, XElement layerElement)
        {
            var name = layerElement.Attribute("name").Value;
            var mapSize = MathUtil.ParseSize(layerElement.Element("MapSize").Value);
            var hexSize = MathUtil.ParseSize(layerElement.Element("HexSize").Value);
            var edgeLength = int.Parse(layerElement.Element("EdgeLength").Value);
            var hexReferences = GetHexReferences(gameResourceManager, layerElement.Element("HexDefinitionReferences")).ToList();
            var hexes = GetRowsFromXml(layerElement.Element("Hexes"));

            var layer = new HexLayer(name, mapSize, hexSize, edgeLength);
            XmlRepository.BaseFromXml(layer, layerElement);

            int x = 0;
            foreach (var row in hexes)
            {
                int y = 0;
                foreach (var element in row)
                {
                    layer[x, y++] = hexReferences.Single(r => r.Id == element).Definition;
                }

                x++;
            }

            return layer;
        }

        public static IEnumerable<object> GetXml(HexSheet hexSheet)
        {
            yield return new XElement("HexSize", hexSheet.HexSize);
            yield return new XElement("Definitions", hexSheet.Definitions.Select(d => GetHexDefinitionXml(d.Value)));
        }

        public static HexSheet FromXml(XElement sheetElement, string name, Texture texture)
        {
            var hexSize = MathUtil.ParseSize(sheetElement.Element("HexSize").Value);
            var hexSheet = new HexSheet(texture, name, hexSize);

            foreach (var definitionElement in sheetElement.Elements("Definitions").Elements())
            {
                var hexDefinition = FromXml(definitionElement, hexSheet);
                hexSheet.AddHexDefinition(hexDefinition);
            }

            return hexSheet;
        }

        private static IEnumerable<HexReference> CreateHexReferences(HexLayer hexLayer)
        {
            var hexDefinitions = new List<HexDefinition>();
            for (var i = 0; i < hexLayer.MapSize.Width; i++)
                for (var j = 0; j < hexLayer.MapSize.Height; j++)
                {
                    hexDefinitions.Add(hexLayer[i, j]);
                }

            return hexDefinitions
                .Distinct()
                .Select((x, i) => new HexReference { Id = i, Definition = x });
        }

        private static IEnumerable<XElement> GetRowsXml(HexLayer hexLayer, List<HexReference> hexReferences)
        {
            for (var i = 0; i < hexLayer.MapSize.Width; i++)
            {
                var row = new int[hexLayer.MapSize.Height];
                for (var j = 0; j < hexLayer.MapSize.Height; j++)
                {
                    row[j] = hexReferences.Single(x => x.Definition == hexLayer[i, j]).Id;
                }

                yield return new XElement("Row", string.Join(", ", row));
            }
        }

        private static IEnumerable<IEnumerable<int>> GetRowsFromXml(XElement rowsElement)
        {
            return rowsElement.Elements()
                .Select(rowElement => rowElement.Value.Split(',').Select(x => int.Parse(x.Trim())));
        }

        private static IEnumerable<HexReference> GetHexReferences(GameResourceManager gameResourceManager, XElement hexReferencesElement)
        {
            return hexReferencesElement.Elements()
                .Select(x => new HexReference
                {
                    Id = int.Parse(x.Attribute("id").Value),
                    Definition = gameResourceManager
                        .GetHexSheet(x.Attribute("sheetName").Value)
                        .Definitions[x.Attribute("name").Value]
                });
        }

        private static object GetHexDefinitionXml(HexDefinition hexDefinition)
        {
            return new XElement("HexDefinition",
                new XAttribute("name", hexDefinition.Name),
                new XAttribute("rectangle", hexDefinition.Rectangle));
        }

        private static HexDefinition FromXml(XElement definitionElement, HexSheet hexSheet)
        {
            var name = definitionElement.Attribute("name").Value;
            var rectangle = MathUtil.ParseRectangle(definitionElement.Attribute("rectangle").Value);

            return new HexDefinition(hexSheet, name, rectangle);
        }

        internal struct HexReference
        {
            public int Id { get; set; }

            public HexDefinition Definition { get; set; }
        }
    }
}