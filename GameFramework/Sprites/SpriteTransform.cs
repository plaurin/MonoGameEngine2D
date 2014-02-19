namespace GameFramework.Sprites
{
    public class SpriteTransform
    {
        private readonly SpriteTransform innerTransform;

        public SpriteTransform(SpriteTransform innerTransform = null,
            Vector? translation = null, float rotation = 0.0f, float scale = 1.0f, Color? color = null)
        {
            this.innerTransform = innerTransform;

            this.Translation = translation.HasValue ? translation.Value : Vector.Zero;
            this.Rotation = rotation;
            this.Scale = scale;
            this.Color = color.HasValue ? color.Value : Color.White;
        }

        public SpriteTransform(SpriteTransform firstTransform, SpriteTransform secondTransform)
            : this(firstTransform, secondTransform.Translation, secondTransform.Rotation, secondTransform.Scale, secondTransform.Color)
        {
        }

        public Vector Translation { get; private set; }

        public float Rotation { get; private set; }

        public float Scale { get; private set; }

        public Color Color { get; private set; }

        public static SpriteTransform Identity
        {
            get { return new SpriteTransform(); }
        }

        public SpriteTransform GetFinal()
        {
            var rotation = 0.0f;
            var scale = 1.0f;
            var translation = Vector.Zero;

            //var color = Color.White;
            var r = 1.0f;
            var g = 1.0f;
            var b = 1.0f;
            var a = 1.0f;

            var currentTransform = this;
            while (currentTransform != null)
            {
                translation = Vector.Zero.TranslatePolar(currentTransform.Rotation, translation);
                translation *= currentTransform.Scale;
                translation += currentTransform.Translation;

                scale *= currentTransform.Scale;

                rotation += currentTransform.Rotation;

                r *= currentTransform.Color.R / 255.0f;
                g *= currentTransform.Color.G / 255.0f;
                b *= currentTransform.Color.B / 255.0f;
                a *= currentTransform.Color.A / 255.0f;

                currentTransform = currentTransform.innerTransform;
            }

            return new SpriteTransform 
            { 
                Rotation = rotation, Scale = scale, Translation = translation, 
                Color = new Color((int)(r * 255), (int)(g * 255), (int)(b * 255), (int)(a * 255)) 
            };
        }
    }
}