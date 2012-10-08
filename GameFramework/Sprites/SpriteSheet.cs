using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

using GameFramework.Cameras;
using GameFramework.Scenes;
using GameFramework.Sheets;

namespace GameFramework.Sprites
{
    public class SpriteSheet : SheetBase
    {
        private readonly IDictionary<string, Rectangle> definitions;

        public SpriteSheet(Texture texture, string name)
            : base(texture, name)
        {
            this.definitions = new Dictionary<string, Rectangle>();
        }

        public void CreateSpriteDefinition(string spriteName, Rectangle spriteRectangle)
        {
            this.definitions.Add(spriteName, spriteRectangle);
        }

        public void Draw(DrawContext drawContext, Camera camera, Point mapOffset, Vector parallaxScrollingVector, Sprite sprite)
        {
            var source = this.definitions[sprite.SpriteName];
            var destination = 
                new Rectangle(
                    mapOffset.X + sprite.Position.X,
                    mapOffset.Y + sprite.Position.Y, 
                    source.Width, 
                    source.Height)
                .Scale(camera.ZoomFactor)
                .Translate(camera.GetSceneTranslationVector(parallaxScrollingVector));

            drawContext.DrawImage(this.Texture, source, destination);
        }

        public HitBase GetHit(Point position, Camera camera, Point mapOffset, Vector parallaxScrollingVector, Sprite sprite)
        {
            var source = this.definitions[sprite.SpriteName];
            var spriteRectangle = 
                new Rectangle(
                    mapOffset.X + sprite.Position.X,
                    mapOffset.X + sprite.Position.Y, 
                    source.Width, 
                    source.Height)
                .Scale(camera.ZoomFactor)
                .Translate(camera.GetSceneTranslationVector(parallaxScrollingVector));

            return spriteRectangle.Intercept(position) 
                ? new SpriteHit(sprite) 
                : null;
        }

        protected override IEnumerable<object> GetXml()
        {
            yield return new XElement("Definitions", this.definitions.Select(d => 
                new XElement("Definition",
                    new XAttribute("name", d.Key),
                    new XAttribute("rectangle", d.Value))));
        }

        public static SpriteSheet FromXml(XElement sheetElement, string name, Texture texture)
        {
            var spriteSheet = new SpriteSheet(texture, name);

            foreach (var definitionElement in sheetElement.Elements("Definitions").Elements())
            {
                spriteSheet.CreateSpriteDefinition(
                    definitionElement.Attribute("name").Value,
                    MathUtil.ParseRectangle(definitionElement.Attribute("rectangle").Value));
            }

            return spriteSheet;
        }
    }
}