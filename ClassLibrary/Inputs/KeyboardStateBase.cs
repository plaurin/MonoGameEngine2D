using System;

namespace ClassLibrary.Inputs
{
    public abstract class KeyboardStateBase
    {
        public abstract bool IsKeyDown(Keys arg);
    }
}