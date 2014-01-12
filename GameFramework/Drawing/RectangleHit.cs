using GameFramework.Scenes;

namespace GameFramework.Drawing
{
    public class RectangleHit : HitBase
    {
        public RectangleHit(RectangleElement rectangleElement)
        {
            this.RectangleElement = rectangleElement;
        }

        public RectangleElement RectangleElement { get; private set; }
    }
}