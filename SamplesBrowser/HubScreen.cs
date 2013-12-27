using System;
using GameFramework;
using GameFramework.Cameras;
using GameFramework.Inputs;
using GameFramework.Scenes;
using GameFramework.Screens;

namespace SamplesBrowser
{
    public class HubScreen : ScreenBase
    {
        public override void Initialize(Camera camera)
        {
        }

        public override InputConfiguration GetInputConfiguration()
        {
            return new InputConfiguration();
        }

        public override void LoadContent(GameResourceManager resourceManager)
        {
        }

        public override void Update(IGameTiming gameTime)
        {
        }

        public override Scene GetScene()
        {
            return new Scene("HubScene");
        }
    }
}
