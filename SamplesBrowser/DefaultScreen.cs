using System;
using FxBase;
using GameFramework;
using GameFramework.Cameras;
using GameFramework.Inputs;
using GameFramework.Screens;
using Scene = GameFramework.Scenes.Scene;

namespace SamplesBrowser
{
    public class DefaultScreen : ScreenBase
    {
        //public MyScene()
        //{
        //    this.Entities.Add(new Line { Point1 = new Vector2(10, 100), Point2 = new Vector2(100, 100) });
        //    this.Entities.Add(new Line { Point1 = new Vector2(100, 10), Point2 = new Vector2(100, 100) });

        //    this.Entities.Add(new Sprite { Position = new Vector2(50, 50), TexturePath = "LinkSheet.png"});
        //}
        public override void Initialize(Camera camera)
        {
            throw new NotImplementedException();
        }

        public override InputConfiguration GetInputConfiguration()
        {
            throw new NotImplementedException();
        }

        public override void LoadContent(GameResourceManager resourceManager)
        {
            throw new NotImplementedException();
        }

        public override void Update(IGameTiming gameTime)
        {
            throw new NotImplementedException();
        }

        public override Scene GetScene()
        {
            throw new NotImplementedException();
        }
    }
}
