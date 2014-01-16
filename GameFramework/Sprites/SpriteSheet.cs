using System;
using System.Collections.Generic;
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

        public IDictionary<string, Rectangle> Definitions
        {
            get { return this.definitions; }
        }

        public void CreateSpriteDefinition(string spriteName, Rectangle spriteRectangle)
        {
            this.Definitions.Add(spriteName, spriteRectangle);
        }

        public void Draw(DrawContext drawContext, Camera camera, Point mapOffset, Vector parallaxScrollingVector, Sprite sprite)
        {
            var source = this.Definitions[sprite.SpriteName];
            var destination = 
                new Rectangle(
                    mapOffset.X + sprite.Position.X,
                    mapOffset.Y + sprite.Position.Y, 
                    source.Width, 
                    source.Height)
                .Scale(camera.ZoomFactor)
                .Translate(camera.GetSceneTranslationVector(parallaxScrollingVector).ToPoint());

            drawContext.DrawImage(this.Texture, source, destination);
        }

        public HitBase GetHit(Vector position, Camera camera, Point mapOffset, Vector parallaxScrollingVector, Sprite sprite)
        {
            var source = this.Definitions[sprite.SpriteName];
            var spriteRectangle = 
                new Rectangle(
                    mapOffset.X + sprite.Position.X,
                    mapOffset.X + sprite.Position.Y, 
                    source.Width, 
                    source.Height)
                .Scale(camera.ZoomFactor)
                .Translate(camera.GetSceneTranslationVector(parallaxScrollingVector).ToPoint());

            return spriteRectangle.Intercept(position.ToPoint()) 
                ? new SpriteHit(sprite) 
                : null;
        }
    }
}