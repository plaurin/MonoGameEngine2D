namespace GameFramework
{
    public class WorldTransform
    {
        private readonly WorldTransform innTransform;

        private Vector offset;
        private Vector parallaxScrollingVector;

        private WorldTransform()
        {
        }

        private WorldTransform(WorldTransform innTransform)
        {
            this.innTransform = innTransform;
        }

        public Vector Offset
        {
            get { return this.offset; }
        }

        public Vector ParallaxScrollingVector
        {
            get { return this.parallaxScrollingVector; }
        }

        public static WorldTransform New
        {
            get { return new WorldTransform(); }
        }

        public WorldTransform Compose(Vector newOffset, Vector newParallaxScrollingVector)
        {
            var newTransform = new WorldTransform(this)
            {
                offset = newOffset,
                parallaxScrollingVector = newParallaxScrollingVector
            };

            return newTransform;
        }
    }
}