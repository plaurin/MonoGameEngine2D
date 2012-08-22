using System;

using ClassLibrary.Cameras;
using ClassLibrary.Inputs;
using ClassLibrary.Scenes;

namespace ClassLibrary.Screens
{
    public abstract class ScreenBase
    {
        public virtual void Initialize(Camera camera)
        {
        }

        public abstract InputConfiguration GetInputConfiguration();

        public virtual void LoadContent(GameResourceManager resourceManager)
        {
        }

        public virtual void Update(double elapsedSeconds, int fps)
        {
        }

        public abstract Scene GetScene();

    }
}