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
                Rotation = rotation,
                Scale = scale,
                Translation = translation,
                Color = new Color((int)(r * 255), (int)(g * 255), (int)(b * 255), (int)(a * 255))
            };
        }

        public override string ToString()
        {
            return string.Format("T: {0}; R: {1}; S: {2}, C: {3}", this.Translation, this.Rotation, this.Scale, this.Color);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return this.Equals((SpriteTransform)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = this.Translation.GetHashCode();
                hashCode = (hashCode * 397) ^ this.Rotation.GetHashCode();
                hashCode = (hashCode * 397) ^ this.Scale.GetHashCode();
                hashCode = (hashCode * 397) ^ this.Color.GetHashCode();
                hashCode = (hashCode * 397) ^ (this.innerTransform != null ? this.innerTransform.GetHashCode() : 0);
                return hashCode;
            }
        }

        protected bool Equals(SpriteTransform other)
        {
            return this.Translation.Equals(other.Translation) 
                   && this.Rotation.Equals(other.Rotation) 
                   && this.Scale.Equals(other.Scale) 
                   && this.Color.Equals(other.Color) 
                   && object.Equals(this.innerTransform, other.innerTransform);
        }
    }
}