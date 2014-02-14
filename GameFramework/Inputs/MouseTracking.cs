using System;

using GameFramework.Cameras;

namespace GameFramework.Inputs
{
    public class MouseTracking
    {
        private readonly ICamera camera;

        private Action<MouseStateBase, IGameTiming> mouseMoveAction;

        public MouseTracking(ICamera camera)
        {
            this.camera = camera;
        }

        public void OnMove(Action<MouseStateBase, IGameTiming> moveAction)
        {
            this.mouseMoveAction = moveAction;
        }

        public void Update(MouseStateBase mouseState, IGameTiming gameTime)
        {
            var cameraAdjutedMouseState = mouseState.AdjustToCamera(this.camera);

            this.mouseMoveAction.Invoke(cameraAdjutedMouseState, gameTime);
        }
    }
}