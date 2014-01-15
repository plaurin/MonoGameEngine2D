using System;
using GameFramework.Cameras;

namespace GameFramework.Inputs
{
    public class TouchTracking
    {
        private readonly Camera camera;
        private Action<TouchStateBase, IGameTiming> touchAction;

        public TouchTracking(Camera camera)
        {
            this.camera = camera;
        }

        public void OnTouch(Action<TouchStateBase, IGameTiming> moveAction)
        {
            this.touchAction = moveAction;
        }

        public void Update(TouchStateBase touchState, IGameTiming gameTime)
        {
            var cameraAdjutedMouseState = touchState.AdjustToCamera(this.camera);

            this.touchAction.Invoke(cameraAdjutedMouseState, gameTime);
        }
    }
}