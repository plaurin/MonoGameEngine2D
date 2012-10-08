using System;

using GameFramework;
using GameFramework.Cameras;
using GameFramework.Inputs;
using GameFramework.Scenes;
using GameFramework.Screens;

namespace ShootEmUpGameDomain
{
    public class DefaultScreen : ScreenBase
    {
        private Camera camera;

        private InputConfiguration inputConfiguration;

        private GameResourceManager gameResourceManager;

        private Scene scene;

        public override void Initialize(Camera theCamera)
        {
            this.camera = theCamera;

            this.camera.Center = CameraCenter.WindowTopLeft;
        }

        public override InputConfiguration GetInputConfiguration()
        {
            this.inputConfiguration = new InputConfiguration();

            return this.inputConfiguration;
        }

        public override void LoadContent(GameResourceManager resourceManager)
        {
            this.gameResourceManager = resourceManager;
        }

        public override void Update(double elapsedSeconds, int fps)
        {
        }

        public override Scene GetScene()
        {
            this.scene = new Scene("MainScene");

            return this.scene;
        }
    }
}
