namespace GameFramework.Sprites
{
    public class SpriteDefinition : ISpriteTemplate
    {
        public SpriteDefinition(SpriteSheet spriteSheet, string name, RectangleInt rectangle, Vector? origin)
        {
            this.SpriteSheet = spriteSheet;
            this.Name = name;
            this.Rectangle = rectangle;
            this.Origin = origin.HasValue ? origin.Value : Vector.Zero;
        }

        public SpriteSheet SpriteSheet { get; private set; }

        public string Name { get; private set; }

        public RectangleInt Rectangle { get; private set; }

        public Vector Origin { get; private set; }

        public SpriteBase CreateInstance()
        {
            return new Sprite(this.SpriteSheet, this.Name);
        }
    }
}