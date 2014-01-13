using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using GameFramework.Hexes;

namespace GameFramework.Repository
{
    public class HexXmlRepository
    {
        public static XElement ToXml(HexMap hexMap)
        {
            var hexReferences = Enumerable.ToList<HexMap.HexReference>(CreateHexReferences(hexMap));

            return new XElement("HexMap", XmlRepository.MapBaseToXml(hexMap),
                new XElement("MapSize", hexMap.MapSize),
                new XElement("HexSize", hexMap.HexSize),
                new XElement("EdgeLength", hexMap.TopEdgeLength),
                new XElement("HexDefinitionReferences", hexReferences.Select(x =>
                    new XElement("Reference",
                        new XAttribute("id", x.Id),
                        new XAttribute("sheetName", x.Definition.SheetName),
                        new XAttribute("name", x.Definition.Name)))),
                new XElement("Hexes", GetRowsXml(hexMap, hexReferences)));
        }

        private static IEnumerable<HexMap.HexReference> CreateHexReferences(HexMap hexMap)
        {
            var hexDefinitions = new List<HexDefinition>();
            for (var i = 0; i < hexMap.MapSize.Width; i++)
                for (var j = 0; j < hexMap.MapSize.Height; j++)
                {
                    hexDefinitions.Add(hexMap[i, j]);
                }

            return hexDefinitions
                .Distinct()
                .Select((x, i) => new HexMap.HexReference { Id = i, Definition = x });
        }

        private static IEnumerable<XElement> GetRowsXml(HexMap hexMap, List<HexMap.HexReference> hexReferences)
        {
            for (var i = 0; i < hexMap.MapSize.Width; i++)
            {
                var row = new int[hexMap.MapSize.Height];
                for (var j = 0; j < hexMap.MapSize.Height; j++)
                {
                    row[j] = hexReferences.Single(x => x.Definition == hexMap[i, j]).Id;
                }

                yield return new XElement("Row", string.Join(", ", row));
            }
        }

        public static HexMap HexMapFromXml(GameResourceManager gameResourceManager, XElement mapElement)
        {
            var name = mapElement.Attribute("name").Value;
            var mapSize = MathUtil.ParseSize(mapElement.Element("MapSize").Value);
            var hexSize = MathUtil.ParseSize(mapElement.Element("HexSize").Value);
            var edgeLength = int.Parse(mapElement.Element("EdgeLength").Value);
            var hexReferences = Enumerable.ToList<HexMap.HexReference>(GetHexReferences(gameResourceManager, mapElement.Element("HexDefinitionReferences")));
            var hexes = GetRowsFromXml(mapElement.Element("Hexes"));

            var map = new HexMap(name, mapSize, hexSize, edgeLength);
            map.BaseFromXml(mapElement);

            int x = 0;
            foreach (var row in hexes)
            {
                int y = 0;
                foreach (var element in row)
                {
                    map[x, y++] = hexReferences.Single(r => r.Id == element).Definition;
                }

                x++;
            }

            return map;
        }

        private static IEnumerable<IEnumerable<int>> GetRowsFromXml(XElement rowsElement)
        {
            return rowsElement.Elements()
                .Select(rowElement => rowElement.Value.Split(',').Select(x => int.Parse(x.Trim())));
        }

        private static IEnumerable<HexMap.HexReference> GetHexReferences(GameResourceManager gameResourceManager, XElement hexReferencesElement)
        {
            return hexReferencesElement.Elements()
                .Select(x => new HexMap.HexReference
                {
                    Id = int.Parse(x.Attribute("id").Value),
                    Definition = gameResourceManager
                        .GetHexSheet(x.Attribute("sheetName").Value)
                        .Definitions[x.Attribute("name").Value]
                });
        }

        public static IEnumerable<object> GetXml(HexSheet hexSheet)
        {
            yield return new XElement("HexSize", hexSheet.HexSize);
            yield return new XElement("Definitions", hexSheet.Definitions.Select(d => d.Value.GetXml()));
        }

        public static HexSheet FromXml(XElement sheetElement, string name, Texture texture)
        {
            var hexSize = MathUtil.ParseSize(sheetElement.Element("HexSize").Value);
            var hexSheet = new HexSheet(texture, name, hexSize);

            foreach (var definitionElement in sheetElement.Elements("Definitions").Elements())
            {
                var hexDefinition = HexDefinition.FromXml(definitionElement, hexSheet);
                hexSheet.AddHexDefinition(hexDefinition);
            }

            return hexSheet;
        }
    }
}