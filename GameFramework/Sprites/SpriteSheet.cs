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

        public int Draw(IDrawContext drawContext, Vector layerOffset, Vector parallaxScrollingVector, 
            Sprite sprite, CameraMode cameraMode)
        {
            var source = this.Definitions[sprite.SpriteName];
            var destination = new Rectangle(layerOffset.X + sprite.Position.X, layerOffset.Y + sprite.Position.Y,
                source.Width, source.Height);

            if (cameraMode == CameraMode.Follow)
            {
                destination = destination
                    .Scale(drawContext.Camera.ZoomFactor)
                    .Translate(drawContext.Camera.GetSceneTranslationVector(parallaxScrollingVector));
            }

            if (drawContext.Camera.Viewport.IsVisible(destination))
            {
                drawContext.DrawImage(new DrawImageParams
                {
                    Texture = this.Texture,
                    Source = source,
                    Destination = destination,
                    Rotation = sprite.Rotation,
                    Origin = sprite.Origin
                });

                return 1;
            }

            return 0;
        }

        public HitBase GetHit(Vector position, Camera camera, Vector layerOffset, Vector parallaxScrollingVector, Sprite sprite)
        {
            var source = this.Definitions[sprite.SpriteName];
            var spriteRectangle = 
                new Rectangle(
                    layerOffset.X + sprite.Position.X,
                    layerOffset.X + sprite.Position.Y,
                    source.Width, 
                    source.Height)
                .Scale(camera.ZoomFactor)
                .Translate(camera.GetSceneTranslationVector(parallaxScrollingVector));

            return spriteRectangle.Intercept(position) 
                ? new SpriteHit(sprite) 
                : null;
        }
    }
}