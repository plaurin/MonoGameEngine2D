using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

using ClassLibrary.Cameras;

namespace ClassLibrary.Sprites
{
    public abstract class SpriteSheet
    {
        private readonly IDictionary<string, Rectangle> definitions;

        protected SpriteSheet(Texture texture, string name)
        {
            this.Texture = texture;
            this.Name = name;

            this.definitions = new Dictionary<string, Rectangle>();
        }

        public string Name { get; private set; }

        public Texture Texture { get; private set; }

        public void CreateSpriteDefinition(string spriteName, Rectangle spriteRectangle)
        {
            this.definitions.Add(spriteName, spriteRectangle);
        }

        public void Draw(DrawContext drawContext, Camera camera, Vector parallaxScrollingVector, Sprite sprite)
        {
            var source = this.definitions[sprite.SpriteName];
            var destination = new Rectangle(sprite.Position.X, sprite.Position.Y, source.Width, source.Height)
                .Scale(camera.ZoomFactor)
                .Translate(camera.GetSceneTranslationVector(parallaxScrollingVector));

            this.DoDraw(drawContext, camera, source, destination);
            //spriteBatch.Draw(this.texture, destination, source, Color.White);
        }

        public XElement GetXml()
        {
            return new XElement("SpriteSheet",
                new XAttribute("name", this.Name),
                new XElement("Texture", this.Texture.Name),
                new XElement("Definitions", this.definitions.Select(d => 
                    new XElement("Definition",
                        new XAttribute("name", d.Key),
                        new XAttribute("rectangle", d.Value)))));
        }

        protected abstract void DoDraw(DrawContext drawContext, Camera camera, Rectangle source, Rectangle destination);
    }
}