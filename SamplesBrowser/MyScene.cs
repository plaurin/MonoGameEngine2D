using System;
using FxBase;

namespace SamplesBrowser
{
    public class MyScene : Scene
    {
        public MyScene()
        {
            this.Entities.Add(new Line { Point1 = new Vector2(10, 100), Point2 = new Vector2(100, 100) });
            this.Entities.Add(new Line { Point1 = new Vector2(100, 10), Point2 = new Vector2(100, 100) });

            this.Entities.Add(new Sprite { Position = new Vector2(50, 50), TexturePath = "LinkSheet.png"});
        }
    }
}
