using System;
using System.Collections.Generic;
using System.Linq;

namespace GameFramework.Inputs
{
    public class DigitalButton
    {
        private readonly List<KeyboardKeys> mappingKeys;

        private readonly List<MouseButtons> mappingButtons;

        private Action<IGameTiming> buttonDownAction;

        private Action<IGameTiming> buttonUpAction;

        private Action<IGameTiming> buttonClickAction;

        private bool isDown;

        public DigitalButton()
        {
            this.mappingKeys = new List<KeyboardKeys>();
            this.mappingButtons = new List<MouseButtons>();
        }

        public DigitalButton Assign(KeyboardKeys key)
        {
            // Should it replace the assignment?
            this.mappingKeys.Add(key);

            return this;
        }

        public DigitalButton Assign(MouseButtons button)
        {
            // Should it replace the assignment?
            this.mappingButtons.Add(button);

            return this;
        }

        public void MapTo(Action<IGameTiming> downAction, Action<IGameTiming> upAction = null)
        {
            this.buttonDownAction = downAction;
            this.buttonUpAction = upAction;
        }

        public void MapClickTo(Action<IGameTiming> clickAction)
        {
            this.buttonClickAction = clickAction;
        }

        public void Update(KeyboardStateBase keyboardState, MouseStateBase mouseState, IGameTiming gameTime)
        {
            var previousDown = this.isDown;

            this.isDown = this.mappingKeys.Any(keyboardState.IsKeyDown)
                || this.mappingButtons.Any(mouseState.IsButtonDown);

            if (this.isDown && this.buttonDownAction != null) this.buttonDownAction.Invoke(gameTime);
            if (!this.isDown && this.buttonUpAction != null) this.buttonUpAction.Invoke(gameTime);

            if (this.buttonClickAction != null && previousDown && !this.isDown) this.buttonClickAction.Invoke(gameTime);
        }
    }
}