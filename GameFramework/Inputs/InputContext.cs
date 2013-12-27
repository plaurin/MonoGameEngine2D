using System;

namespace GameFramework.Inputs
{
    public abstract class InputContext
    {
        public object Keyboard { get; set; }

        public abstract KeyboardStateBase KeyboardGetState();

        public abstract MouseStateBase MouseGetState();
    }
}