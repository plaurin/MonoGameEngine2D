using System;
using System.Collections.Generic;
using System.Linq;

using GameFramework.Cameras;
using GameFramework.Maps;
using GameFramework.Scenes;

namespace GameFramework.Sprites
{
    public class SpriteMap : MapBase
    {
        private readonly List<SpriteBase> sprites;

        public SpriteMap(string name)
            : base(name)
        {
            this.sprites = new List<SpriteBase>();
        }

        internal List<SpriteBase> Sprites
        {
            get { return this.sprites; }
        }

        public void AddSprite(SpriteBase sprite)
        {
            this.Sprites.Add(sprite);
        }

        public void RemoveSprite(SpriteBase sprite)
        {
            this.Sprites.Remove(sprite);
        }

        public override void Draw(DrawContext drawContext, Camera camera)
        {
            foreach (var sprite in this.Sprites)
            {
                sprite.Draw(drawContext, camera, this.Offset, this.ParallaxScrollingVector);
            }
        }

        public override HitBase GetHit(Point position, Camera camera)
        {
            return this.Sprites
                .Select(sprite => sprite.GetHit(position, camera, this.Offset, this.ParallaxScrollingVector))
                .FirstOrDefault(spriteHit => spriteHit != null);
        }

        //public override XElement ToXml()
        //{
        //    return XmlRepository.ToXml(this);
        //}

    }
}
