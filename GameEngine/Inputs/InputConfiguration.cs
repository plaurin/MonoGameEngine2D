using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework.Input;

namespace WindowsGame1.Inputs
{
    public class InputConfiguration
    {
        private readonly Dictionary<string, DigitalButton> digitalButtons;

        public InputConfiguration()
        {
            this.digitalButtons = new Dictionary<string, DigitalButton>();
        }

        public void Update()
        {
            var keyState = Keyboard.GetState();

            foreach (var digitalButton in this.digitalButtons.Values)
            {
                digitalButton.Update(keyState);
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
