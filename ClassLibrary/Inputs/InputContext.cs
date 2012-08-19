using System;

namespace ClassLibrary.Inputs
{
    public abstract class InputContext
    {
        public object Keyboard { get; set; }

        public abstract KeyboardStateBase KeyboardGetState();
    }
}