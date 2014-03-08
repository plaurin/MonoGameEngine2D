using System;

namespace GameFramework.Sprites
{
    public class SpriteTransform : Transform
    {
        public SpriteTransform(Transform innerTransform = null,
            Vector? translation = null, float rotation = 0.0f, float scale = 1.0f, Color? color = null)
            : base(innerTransform, translation, rotation, scale)
        {
            this.Color = color.HasValue ? color.Value : Color.White;
        }

        public SpriteTransform(SpriteTransform innerTransform, SpriteTransform otherTransform)
            : base(innerTransform, otherTransform.Translation, otherTransform.Rotation, otherTransform.Scale)
        {
            this.Color = innerTransform.Color.Multiply(otherTransform.Color);
        }

        public Color Color { get; private set; }

        public static SpriteTransform SpriteIdentity
        {
            get { return new SpriteTransform(); }
        }

        public SpriteTransform GetSpriteFinal()
        {
            return (SpriteTransform)this.GetFinal();
        }

        public override string ToString()
        {
            return string.Format("T: {0}; R: {1}; S: {2}, C: {3}", this.Translation, this.Rotation, this.Scale, this.Color);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = base.GetHashCode();
                hashCode = (hashCode * 397) ^ this.Color.GetHashCode();
                return hashCode;
            }
        }

        protected override Transform CreateIdentity()
        {
            return SpriteIdentity;
        }

        protected override void Update(Transform cumulativeTransform, Transform currentTransform, bool hasRotation)
        {
            base.Update(cumulativeTransform, currentTransform, hasRotation);

            var cumulSprite = (SpriteTransform)cumulativeTransform;
            var currentSprite = currentTransform as SpriteTransform;

            if (currentSprite != null)
                cumulSprite.Color = cumulSprite.Color.Multiply(currentSprite.Color);
        }

        protected bool Equals(SpriteTransform other)
        {
            return base.Equals(other)
                && this.Color.Equals(other.Color);
        }
    }
}