using System;
using System.Collections.Generic;

using ClassLibrary.Cameras;

namespace ClassLibrary.Inputs
{
    public class InputConfiguration
    {
        private readonly Dictionary<string, DigitalButton> digitalButtons;

        private MouseTracking mouseTracking;

        public InputConfiguration()
        {
            this.digitalButtons = new Dictionary<string, DigitalButton>();
        }

        public void Update(InputContext inputContext, double elapsedSeconds)
        {
            var keyState = inputContext.KeyboardGetState();
            var mouseState = inputContext.MouseGetState();

            if (this.mouseTracking != null) 
                this.mouseTracking.Update(mouseState, elapsedSeconds);

            foreach (var digitalButton in this.digitalButtons.Values)
            {
                digitalButton.Update(keyState, mouseState, elapsedSeconds);
            }
        }

        public DigitalButton AddDigitalButton(string name)
        {
            var digitalButton = new DigitalButton();
            this.digitalButtons.Add(name, digitalButton);

            return digitalButton;
        }

        public MouseTracking AddMouseTracking(Camera camera)
        {
            this.mouseTracking = new MouseTracking(camera);
            
            return this.mouseTracking;
        }
    }
}
