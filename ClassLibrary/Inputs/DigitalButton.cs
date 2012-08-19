using System;
using System.Collections.Generic;
using System.Linq;

namespace ClassLibrary.Inputs
{
    public class DigitalButton
    {
        private readonly List<Keys> mappingKeys;

        private Action<float> buttonDownAction;
        private Action<float> buttonUpAction;

        private bool isDown;

        public DigitalButton()
        {
            this.mappingKeys = new List<Keys>();
        }

        public DigitalButton Assign(Keys key)
        {
            this.mappingKeys.Add(key);

            return this;
        }

        public void MapTo(Action<float> downAction, Action<float> upAction = null)
        {
            this.buttonDownAction = downAction;
            this.buttonUpAction = upAction;
        }

        public void Update(KeyboardStateBase keyboardState, float elapsedSeconds)
        {
            this.isDown = this.mappingKeys.Any(keyboardState.IsKeyDown);

            if (this.isDown && this.buttonDownAction != null) this.buttonDownAction.Invoke(elapsedSeconds);
            if (!this.isDown && this.buttonUpAction != null) this.buttonUpAction.Invoke(elapsedSeconds);
        }
    }
}