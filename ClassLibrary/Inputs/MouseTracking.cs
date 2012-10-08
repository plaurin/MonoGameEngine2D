using System;

using GameFramework.Cameras;

namespace GameFramework.Inputs
{
    public class MouseTracking
    {
        private readonly Camera camera;

        private Action<MouseStateBase, double> mouseMoveAction;

        public MouseTracking(Camera camera)
        {
            this.camera = camera;
        }

        public void OnMove(Action<MouseStateBase, double> moveAction)
        {
            this.mouseMoveAction = moveAction;
        }

        public void Update(MouseStateBase mouseState, double elapsedSeconds)
        {
            var cameraAdjutedMouseState = mouseState.AdjustToCamera(this.camera);

            this.mouseMoveAction.Invoke(cameraAdjutedMouseState, elapsedSeconds);
        }
    }
}