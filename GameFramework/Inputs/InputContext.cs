using System;
using System.Collections.Generic;

namespace GameFramework.Inputs
{
    public abstract class InputContext
    {
        public object Keyboard { get; set; }

        public abstract KeyboardStateBase KeyboardGetState();

        public abstract MouseStateBase MouseGetState();

        public abstract TouchStateBase TouchGetState();

        public abstract void UpdateEnabledGestures(IEnumerable<TouchGestureType> touchGestures);
    }
}