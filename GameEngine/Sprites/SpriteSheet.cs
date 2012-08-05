using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using WindowsGame1.Cameras;

namespace WindowsGame1.Sprites
{
    public class SpriteSheet
    {
        private readonly Texture2D texture;
        private readonly IDictionary<string, Rectangle> definitions;

        public SpriteSheet(Texture2D texture, string name)
        {
            this.texture = texture;
            this.Name = name;

            this.definitions = new Dictionary<string, Rectangle>();
        }

        public string Name { get; private set; }

        public void CreateSpriteDefinition(string spriteName, Rectangle spriteRectangle)
        {
            this.definitions.Add(spriteName, spriteRectangle);
        }

        public void Draw(SpriteBatch spriteBatch, Camera camera, Vector2 parallaxScrollingVector, Sprite sprite)
        {
            var source = this.definitions[sprite.SpriteName];
            var destination = new Rectangle(sprite.Position.X, sprite.Position.Y, source.Width, source.Height)
                .Scale(camera.ZoomFactor)
                .Translate(camera.GetSceneTranslationVector(parallaxScrollingVector));

            spriteBatch.Draw(this.texture, destination, source, Color.White);
        }

        public XElement GetXml()
        {
            return new XElement("SpriteSheet",
                new XAttribute("name", this.Name),
                new XElement("Texture", this.texture.Name),
                new XElement("Definitions", this.definitions.Select(d => 
                    new XElement("Definition",
                        new XAttribute("name", d.Key),
                        new XAttribute("rectangle", d.Value)))));
        }
    }
}