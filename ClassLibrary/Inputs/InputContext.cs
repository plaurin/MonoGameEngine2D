using System;

namespace ClassLibrary.Inputs
{
    public class InputContext
    {
        public object Keyboard { get; set; }

        public InputContext KeyboardGetState()
        {
            throw new NotImplementedException();
        }

        public bool IsKeyDown(Keys arg)
        {
            throw new NotImplementedException();
        }
    }
}