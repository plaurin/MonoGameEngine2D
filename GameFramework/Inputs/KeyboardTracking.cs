using System;

namespace GameFramework.Inputs
{
    public interface IKeyboardMapper
    {
        void OnUpdate(Action<KeyboardStateBase, IGameTiming> trackingAction);
    }

    public class KeyboardTracking : IKeyboardMapper
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