using System;

namespace GameFramework.Inputs
{
    public class KeyboardTracking
    {
        private Action<KeyboardStateBase, IGameTiming> keyboardTrackingAction;

        public void OnUpdate(Action<KeyboardStateBase, IGameTiming> trackingAction)
        {
            this.keyboardTrackingAction = trackingAction;
        }

        public void Update(KeyboardStateBase keyboardStateState, IGameTiming gameTime)
        {
            this.keyboardTrackingAction.Invoke(keyboardStateState, gameTime);
        }
    }
}