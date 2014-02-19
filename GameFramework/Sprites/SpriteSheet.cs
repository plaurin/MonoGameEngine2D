using System;
using System.Collections.Generic;
using System.Linq;
using GameFramework.Cameras;
using GameFramework.Scenes;
using GameFramework.Sheets;

namespace GameFramework.Sprites
{
    public class SpriteSheet : SheetBase, IComposite
    {
        private readonly IDictionary<string, SpriteDefinition> definitions;
        private readonly IDictionary<string, SpriteAnimationTemplate> animationTemplates;
        private readonly IDictionary<string, SpriteCompositeTemplate> compositeTemplates;

        public SpriteSheet(Texture texture, string name)
            : base(texture, name)
        {
            this.definitions = new Dictionary<string, SpriteDefinition>();
            this.animationTemplates = new Dictionary<string, SpriteAnimationTemplate>();
            this.compositeTemplates = new Dictionary<string, SpriteCompositeTemplate>();
        }

        public IEnumerable<SpriteDefinition> Definitions
        {
            get { return this.definitions.Values; }
        }

        public IEnumerable<SpriteAnimationTemplate> AnimationTemplates
        {
            get { return this.animationTemplates.Values; }
        }

        public IEnumerable<SpriteCompositeTemplate> CompositeTemplates
        {
            get { return this.compositeTemplates.Values; }
        }

        public SpriteDefinition AddSpriteDefinition(string spriteName, RectangleInt spriteRectangle, Vector? origin = null)
        {
            var spriteDefinition = new SpriteDefinition(this, spriteName, spriteRectangle, origin);
            this.definitions.Add(spriteName, spriteDefinition);
            return spriteDefinition;
        }

        public SpriteDefinition GetSpriteDefinition(string spriteName)
        {
            return this.definitions[spriteName];
        }

        public Sprite CreateSprite(string spriteName)
        {
            return (Sprite)this.definitions[spriteName].CreateInstance();
        }

        public SpriteAnimationTemplate AddSpriteAnimationTemplate(string animationName)
        {
            var template = new SpriteAnimationTemplate(animationName);
            this.animationTemplates.Add(animationName, template);
            return template;
        }

        public SpriteAnimation CreateSpriteAnimation(string animationName)
        {
            return (SpriteAnimation)this.animationTemplates[animationName].CreateInstance();
        }

        public SpriteCompositeTemplate AddSpriteCompositeTemplate(string compositeName)
        {
            var template = new SpriteCompositeTemplate(compositeName);
            this.compositeTemplates.Add(compositeName, template);
            return template;
        }

        public SpriteComposite CreateSpriteComposite(string compositeName)
        {
            return (SpriteComposite)this.animationTemplates[compositeName].CreateInstance();
        }

        public int Draw(IDrawContext drawContext, Sprite sprite, SpriteTransform transform)
        {
            var source = this.definitions[sprite.SpriteName];

            var finalTransform = new SpriteTransform(transform, sprite.Position, sprite.Rotation, sprite.Scale, sprite.Color)
                .GetFinal();

            var destination = new Rectangle(
                finalTransform.Translation.X, finalTransform.Translation.Y,
                source.Rectangle.Width * finalTransform.Scale, source.Rectangle.Height * finalTransform.Scale);

            if (drawContext.Camera.Viewport.IsVisible(destination))
            {
                var origin = sprite.Origin.HasValue ? sprite.Origin.Value : source.Origin;

                drawContext.DrawImage(new DrawImageParams
                {
                    Texture = this.Texture,
                    Source = source.Rectangle,
                    Destination = destination,
                    Rotation = finalTransform.Rotation,
                    Origin = origin,
                    Color = finalTransform.Color,
                    ImageEffect = GetImageEffect(sprite)
                });

                return 1;
            }

            return 0;
        }

        public HitBase GetHit(Vector position, ICamera camera, WorldTransform worldTransform, Sprite sprite)
        {
            var source = this.definitions[sprite.SpriteName];
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

        public IEnumerable<object> Children
        {
            get
            {
                return this.Definitions.Cast<object>()
                .Concat(this.AnimationTemplates)
                .Concat(this.CompositeTemplates);
            }
        }

        private static ImageEffect GetImageEffect(Sprite sprite)
        {
            if (sprite.FlipHorizontally && !sprite.FlipVertically) return ImageEffect.FlipHorizontally;
            if (sprite.FlipHorizontally && sprite.FlipVertically) return ImageEffect.FlipHorizontally | ImageEffect.FlipVertically;
            if (!sprite.FlipHorizontally && sprite.FlipVertically) return ImageEffect.FlipVertically;

            return ImageEffect.None;
        }
    }
}