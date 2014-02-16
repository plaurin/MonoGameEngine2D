namespace GameFramework.Sprites
{
    public class SpriteDefinition
    {
        public SpriteDefinition(string name, RectangleInt rectangle, Vector? origin)
        {
            this.Name = name;
            this.Rectangle = rectangle;
            this.Origin = origin.HasValue ? origin.Value : Vector.Zero;
        }

        public string Name { get; private set; }

        public RectangleInt Rectangle { get; private set; }

        public Vector Origin { get; private set; }
    }
}