using System;
using System.Collections.Generic;

namespace GameFramework.Inputs
{
    public abstract class KeyboardStateBase
    {
        public abstract bool IsKeyDown(KeyboardKeys key);

        public abstract IEnumerable<KeyboardKeys> PressedKeys();
    }
}