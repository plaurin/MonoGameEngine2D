using System;
using System.Collections.Generic;
using System.Linq;
using GameFramework.Cameras;

namespace GameFramework.Inputs
{
    public class InputConfiguration
    {
        private readonly Dictionary<string, DigitalButton> digitalButtons;
        private readonly Dictionary<string, InputEvent> inputEvents;

        private MouseTracking mouseTracking;
        private TouchTracking touchTracking;

        public InputConfiguration()
        {
            this.digitalButtons = new Dictionary<string, DigitalButton>();
            this.inputEvents = new Dictionary<string, InputEvent>();
            this.EnabledGestures = Enumerable.Empty<TouchGesture>();
        }

        public IEnumerable<TouchGesture> EnabledGestures { get; private set; }

        public bool EnabledGesturesUpdated { get; set; }

        public void Update(InputContext inputContext, IGameTiming gameTime)
        {
            var keyState = inputContext.KeyboardGetState();
            var mouseState = inputContext.MouseGetState();
            var touchState = inputContext.TouchGetState();

            if (this.mouseTracking != null)
                this.mouseTracking.Update(mouseState, gameTime);

            foreach (var digitalButton in this.digitalButtons.Values)
            {
                digitalButton.Update(keyState, mouseState, gameTime);
            }

            if (this.touchTracking != null)
            {
                this.touchTracking.Update(touchState, gameTime);
            }

            foreach (var inputEvent in this.inputEvents.Values)
            {
                inputEvent.Update(touchState, gameTime);
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

        public InputEvent AddEvent(string name)
        {
            var inputEvent = new InputEvent();
            this.inputEvents.Add(name, inputEvent);

            return inputEvent;
        }

        public InputEvent GetEvent(string name)
        {
            return this.inputEvents[name];
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

        public void EnableGesture(params TouchGesture[] touchGestures)
        {
            this.EnabledGestures = touchGestures.ToList();
            this.EnabledGesturesUpdated = true;
        }
    }
}
