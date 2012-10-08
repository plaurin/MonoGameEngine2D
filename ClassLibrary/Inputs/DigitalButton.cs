using System;
using System.Collections.Generic;
using System.Linq;

namespace GameFramework.Inputs
{
    public class DigitalButton
    {
        private readonly List<KeyboardKeys> mappingKeys;

        private readonly List<MouseButtons> mappingButtons;

        private Action<float> buttonDownAction;

        private Action<float> buttonUpAction;

        private Action<float> buttonClickAction;

        private bool isDown;

        public DigitalButton()
        {
            this.mappingKeys = new List<KeyboardKeys>();
            this.mappingButtons = new List<MouseButtons>();
        }

        public DigitalButton Assign(KeyboardKeys key)
        {
            this.mappingKeys.Add(key);

            return this;
        }

        public DigitalButton Assign(MouseButtons button)
        {
            this.mappingButtons.Add(button);

            return this;
        }

        public void MapTo(Action<float> downAction, Action<float> upAction = null)
        {
            this.buttonDownAction = downAction;
            this.buttonUpAction = upAction;
        }

        public void MapClickTo(Action<float> clickAction)
        {
            this.buttonClickAction = clickAction;
        }

        public void Update(KeyboardStateBase keyboardState, MouseStateBase mouseState, double elapsedSeconds)
        {
            var previousDown = this.isDown;

            this.isDown = this.mappingKeys.Any(keyboardState.IsKeyDown)
                || this.mappingButtons.Any(mouseState.IsButtonDown);

            if (this.isDown && this.buttonDownAction != null) this.buttonDownAction.Invoke((float)elapsedSeconds);
            if (!this.isDown && this.buttonUpAction != null) this.buttonUpAction.Invoke((float)elapsedSeconds);

            if (this.buttonClickAction != null && previousDown && !this.isDown) this.buttonClickAction.Invoke((float)elapsedSeconds);
        }
    }
}