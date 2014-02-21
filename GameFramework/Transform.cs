using System;

namespace GameFramework
{
    public class Transform
    {
        private readonly Transform innerTransform;

        public Transform(Transform innerTransform = null,
            Vector? translation = null, float rotation = 0.0f, float scale = 1.0f)
        {
            this.innerTransform = innerTransform;

            this.Translation = translation.HasValue ? translation.Value : Vector.Zero;
            this.Rotation = rotation;
            this.Scale = scale;
        }

        public Vector Translation { get; set; }

        public float Rotation { get; set; }

        public float Scale { get; set; }

        public static Transform Identity
        {
            get { return new Transform(); }
        }

        public Transform GetFinal()
        {
            var cumulativeTranform = this.CreateIdentity();

            var currentTransform = this;
            var hasRotation = false;
            while (currentTransform != null)
            {
                if (Math.Abs(currentTransform.Rotation) > 0.001f) hasRotation = true;

                this.Update(cumulativeTranform, currentTransform, hasRotation);
                currentTransform = currentTransform.innerTransform;
            }

            return cumulativeTranform;
        }

        public override string ToString()
        {
            return string.Format("T: {0}; R: {1}; S: {2}", this.Translation, this.Rotation, this.Scale);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return this.Equals((Transform)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = this.Translation.GetHashCode();
                hashCode = (hashCode * 397) ^ this.Rotation.GetHashCode();
                hashCode = (hashCode * 397) ^ this.Scale.GetHashCode();
                hashCode = (hashCode * 397) ^ (this.innerTransform != null ? this.innerTransform.GetHashCode() : 0);
                return hashCode;
            }
        }

        protected virtual Transform CreateIdentity()
        {
            return Identity;
        }

        protected virtual void Update(Transform cumulativeTransform, Transform currentTransform, bool hasRotation)
        {
            if (hasRotation)
            {
                cumulativeTransform.Translation = Vector.Zero.TranslatePolar(currentTransform.Rotation, cumulativeTransform.Translation);
                cumulativeTransform.Translation *= currentTransform.Scale;
                cumulativeTransform.Translation += currentTransform.Translation;
            }
            else
            {
                cumulativeTransform.Translation *= currentTransform.Scale;
                cumulativeTransform.Translation += currentTransform.Translation;
            }

            cumulativeTransform.Scale *= currentTransform.Scale;

            cumulativeTransform.Rotation += currentTransform.Rotation;
        }

        protected bool Equals(Transform other)
        {
            return this.Translation.Equals(other.Translation) 
                   && this.Rotation.Equals(other.Rotation) 
                   && this.Scale.Equals(other.Scale) 
                   && object.Equals(this.innerTransform, other.innerTransform);
        }
    }
}