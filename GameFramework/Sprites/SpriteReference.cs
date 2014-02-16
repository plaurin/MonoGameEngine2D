using GameFramework.Cameras;

namespace GameFramework.Sprites
{
    public class SpriteReference : SpriteBase, IUpdatable
    {
        public SpriteBase Sprite { get; set; }

        public override int Draw(IDrawContext drawContext, Vector layerOffset, Vector parallaxScrollingVector, CameraMode cameraMode)
        {
            if (this.Sprite != null && this.IsVisible)
            {
                // TODO: Fix this with CompositeSprite that adjust underlying sprite position and rotation
                var referenceOffset = layerOffset.Translate(this.Position);
                return this.Sprite.Draw(drawContext, referenceOffset, parallaxScrollingVector, cameraMode);
            }

            return 0;
        }

        public void Update(IGameTiming gameTiming)
        {
            if (this.Sprite != null)
            {
                var updatable = this.Sprite as IUpdatable;
                if (updatable != null) updatable.Update(gameTiming);
            }
        }
    }
}