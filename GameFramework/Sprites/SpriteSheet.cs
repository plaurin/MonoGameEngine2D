using System;
using System.Collections.Generic;
using GameFramework.Cameras;
using GameFramework.Scenes;
using GameFramework.Sheets;

namespace GameFramework.Sprites
{
    public class SpriteSheet : SheetBase
    {
        private readonly IDictionary<string, SpriteDefinition> definitions;

        public SpriteSheet(Texture texture, string name)
            : base(texture, name)
        {
            this.definitions = new Dictionary<string, SpriteDefinition>();
        }

        public IDictionary<string, SpriteDefinition> Definitions
        {
            get { return this.definitions; }
        }

        public SpriteDefinition CreateSpriteDefinition(string spriteName, RectangleInt spriteRectangle, Vector? origin = null)
        {
            var spriteDefinition = new SpriteDefinition(this, spriteName, spriteRectangle, origin);
            this.Definitions.Add(spriteName, spriteDefinition);
            return spriteDefinition;
        }

        public SpriteDefinition GetSpriteDefinition(string spriteName)
        {
            return this.definitions[spriteName];
        }

        public int Draw(IDrawContext drawContext, Vector layerOffset, Vector parallaxScrollingVector, 
            Sprite sprite, CameraMode cameraMode)
        {
            var source = this.Definitions[sprite.SpriteName];
            var destination = new Rectangle(layerOffset.X + sprite.Position.X, layerOffset.Y + sprite.Position.Y,
                source.Rectangle.Width, source.Rectangle.Height);

            if (cameraMode == CameraMode.Follow)
            {
                destination = destination
                    .Scale(drawContext.Camera.ZoomFactor)
                    .Translate(drawContext.Camera.GetSceneTranslationVector(parallaxScrollingVector));
            }

            if (drawContext.Camera.Viewport.IsVisible(destination))
            {
                var origin = sprite.Origin.HasValue ? sprite.Origin.Value : source.Origin;

                drawContext.DrawImage(new DrawImageParams
                {
                    Texture = this.Texture,
                    Source = source.Rectangle,
                    Destination = destination,
                    Rotation = sprite.Rotation,
                    Origin = origin
                });

                return 1;
            }

            return 0;
        }

        public HitBase GetHit(Vector position, ICamera camera, WorldTransform worldTransform, Sprite sprite)
        {
            var source = this.Definitions[sprite.SpriteName];
            var spriteRectangle =
                new Rectangle(
                    worldTransform.Offset.X + sprite.Position.X,
                    worldTransform.Offset.X + sprite.Position.Y,
                    source.Rectangle.Width,
                    source.Rectangle.Height)
                .Scale(camera.ZoomFactor)
                .Translate(camera.GetSceneTranslationVector(worldTransform.ParallaxScrollingVector));

            return spriteRectangle.Intercept(position)
                ? new SpriteHit(sprite)
                : null;
        }
    }
}