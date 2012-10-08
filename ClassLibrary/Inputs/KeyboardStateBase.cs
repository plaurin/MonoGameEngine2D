using System;

namespace GameFramework.Inputs
{
    public abstract class KeyboardStateBase
    {
        public abstract bool IsKeyDown(KeyboardKeys key);
    }
}