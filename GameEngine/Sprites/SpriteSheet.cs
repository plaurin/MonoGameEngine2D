using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WindowsGame1.Sprites
{
    public class SpriteSheet
    {
        private readonly Texture2D texture2D;
        private readonly IDictionary<string, Rectangle> definitions;

        public SpriteSheet(Texture2D texture2D, string sheetName)
        {
            this.texture2D = texture2D;
            this.SheetName = sheetName;

            this.definitions = new Dictionary<string, Rectangle>();
        }

        public string SheetName { get; private set; }

        public void CreateSpriteDefinition(string spriteName, Rectangle spriteRectangle)
        {
            this.definitions.Add(spriteName, spriteRectangle);
        }

        public void Draw(SpriteBatch spriteBatch, Sprite sprite, float scaling)
        {
            var source = this.definitions[sprite.SpriteName];
            var destination = new Rectangle(sprite.Position.X, sprite.Position.Y, source.Width, source.Height)
                .Scale(scaling);

            spriteBatch.Draw(this.texture2D, destination, source, Color.White);
        }
    }
}