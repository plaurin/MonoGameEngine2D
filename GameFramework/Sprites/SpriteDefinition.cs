namespace GameFramework.Sprites
{
    public class SpriteDefinition : ISpriteTemplate, INavigatorMetadataProvider
    {
        internal SpriteDefinition(SpriteSheet spriteSheet, string name, RectangleInt rectangle, Vector? origin)
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

        public NavigatorMetadata GetMetadata()
        {
            // TODO need kind and icon for SpriteDefinition (any kind of definition/template?)
            return new NavigatorMetadata(this.Name);
        }
    }
}