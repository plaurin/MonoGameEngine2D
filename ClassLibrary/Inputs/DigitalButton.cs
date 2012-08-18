using System;
using System.Collections.Generic;
using System.Linq;

namespace ClassLibrary.Inputs
{
    public class DigitalButton
    {
        private readonly List<Keys> mappingKeys;

        private Action buttonDownAction;
        private Action buttonUpAction;

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

        public void MapTo(Action downAction, Action upAction = null)
        {
            this.buttonDownAction = downAction;
            this.buttonUpAction = upAction;
        }

        public void Update(InputContext inputContext)
        {
            this.isDown = this.mappingKeys.Any(inputContext.IsKeyDown);

            if (this.isDown && this.buttonDownAction != null) this.buttonDownAction.Invoke();
            if (!this.isDown && this.buttonUpAction != null) this.buttonUpAction.Invoke();
        }
    }
}