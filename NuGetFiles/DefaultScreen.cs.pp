using System;
using GameFramework;
using GameFramework.Cameras;
using GameFramework.Inputs;
using GameFramework.Scenes;
using GameFramework.Screens;

namespace $rootnamespace$
{
    public class DefaultScreen : SceneBasedScreen
    {
        protected override Camera CreateCamera(Viewport viewport)
        {
            return new Camera(viewport);
        }

        protected override InputConfiguration CreateInputConfiguration()
        {
            return new InputConfiguration();
        }

        protected override Scene CreateScene()
        {
            return new Scene("Default");
        }
    }
}
