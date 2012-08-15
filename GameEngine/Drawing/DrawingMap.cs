using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using WindowsGame1.Cameras;
using WindowsGame1.Maps;

namespace WindowsGame1.Drawing
{
    public class DrawingMap : MapBase
    {
        private readonly GameResourceManager gameResourceManager;

        private readonly List<DrawingElementBase> elements;

        private readonly Texture2D blank;

        public DrawingMap(string name, GameResourceManager gameResourceManager)
            : base(name)
        {
            this.gameResourceManager = gameResourceManager;

            this.elements = new List<DrawingElementBase>();
            this.blank = this.gameResourceManager.GetTexture("WhitePixel");
        }

        public TextElement AddText(DrawingFont drawingFont, string text, Vector2 vector, Color color)
        {
            var textElement = new TextElement(drawingFont, text, vector, color);
            this.elements.Add(textElement);
            return textElement;
        }

        public void AddText(TextElement textElement)
        {
            this.elements.Add(textElement);
        }

        public LineElement AddLine(Vector2 fromVector, Vector2 toVector, int width, Color color)
        {
            var lineElement = new LineElement(this.blank, fromVector, toVector, width, color);
            this.elements.Add(lineElement);

            return lineElement;
        }

        public void AddLine(LineElement lineElement)
        {
            this.elements.Add(lineElement);
        }

        public override void Draw(SpriteBatch spriteBatch, Camera camera)
        {
            foreach (var element in this.elements)
            {
                element.Draw(spriteBatch, camera, this);
            }
        }

        public override XElement ToXml()
        {
            return new XElement("DrawingMap", 
                this.BaseToXml(),
                new XElement("Elements", 
                    this.elements.Select(e => e.ToXml())));
        }

        public static MapBase FromXml(GameResourceManager gameResourceManager, XElement mapElement)
        {
            var name = mapElement.Attribute("name").Value;
            var map = new DrawingMap(name, gameResourceManager);
            map.BaseFromXml(mapElement);

            foreach (var element in mapElement.Element("Elements").Elements())
            {
                switch (element.Name.ToString())
                {
                    case "TextElement":
                        map.AddText(TextElement.FromXml(gameResourceManager, element));
                        break;
                    case "LineElement":
                        map.AddLine(LineElement.FromXml(gameResourceManager, element));
                        break;
                    default:
                        throw new NotImplementedException(element.Name + " is not implemented yet.");
                }
            }

            return map;
        }
    }
}
