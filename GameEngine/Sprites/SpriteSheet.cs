using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using WindowsGame1.Cameras;

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

        public void Draw(SpriteBatch spriteBatch, Camera camera, Vector2 parallaxScrollingVector, Sprite sprite)
        {
            var source = this.definitions[sprite.SpriteName];
            var destination = new Rectangle(sprite.Position.X, sprite.Position.Y, source.Width, source.Height)
                .Scale(camera.ZoomFactor)
                .Translate(camera.GetSceneTranslationVector(parallaxScrollingVector));

            spriteBatch.Draw(this.texture2D, destination, source, Color.White);
        }
    }
}