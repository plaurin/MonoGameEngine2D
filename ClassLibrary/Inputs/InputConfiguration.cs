using System;
using System.Collections.Generic;

namespace ClassLibrary.Inputs
{
    public class InputConfiguration
    {
        private readonly Dictionary<string, DigitalButton> digitalButtons;

        public InputConfiguration()
        {
            this.digitalButtons = new Dictionary<string, DigitalButton>();
        }

        public void Update(InputContext inputContext, float elapsedSeconds)
        {
            var keyState = inputContext.KeyboardGetState();

            foreach (var digitalButton in this.digitalButtons.Values)
            {
                digitalButton.Update(keyState, elapsedSeconds);
            }
        }

        public DigitalButton AddDigitalButton(string name)
        {
            var digitalButton = new DigitalButton();
            this.digitalButtons.Add(name, digitalButton);

            return digitalButton;
        }
    }
}
