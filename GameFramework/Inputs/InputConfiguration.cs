using System;
using System.Collections.Generic;

using GameFramework.Cameras;

namespace GameFramework.Inputs
{
    public class InputConfiguration
    {
        private readonly Dictionary<string, DigitalButton> digitalButtons;

        private MouseTracking mouseTracking;
        private TouchTracking touchTracking;

        public InputConfiguration()
        {
            this.digitalButtons = new Dictionary<string, DigitalButton>();
        }

        public void Update(InputContext inputContext, IGameTiming gameTime)
        {
            var keyState = inputContext.KeyboardGetState();
            var mouseState = inputContext.MouseGetState();

            if (this.mouseTracking != null) 
                this.mouseTracking.Update(mouseState, gameTime);

            foreach (var digitalButton in this.digitalButtons.Values)
            {
                digitalButton.Update(keyState, mouseState, gameTime);
            }

            if (this.touchTracking != null)
            {
                var touchState = inputContext.TouchGetState();
                this.touchTracking.Update(touchState, gameTime);
            }
        }

        public DigitalButton AddDigitalButton(string name)
        {
            var digitalButton = new DigitalButton();
            this.digitalButtons.Add(name, digitalButton);

            return digitalButton;
        }

        public DigitalButton GetDigitalButton(string name)
        {
            return this.digitalButtons[name];
        }

        public MouseTracking AddMouseTracking(Camera camera)
        {
            this.mouseTracking = new MouseTracking(camera);
            
            return this.mouseTracking;
        }

        public TouchTracking AddTouchTracking(Camera camera)
        {
            this.touchTracking = new TouchTracking(camera);

            return this.touchTracking;
        }
    }
}
