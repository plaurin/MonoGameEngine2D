using System;

using ClassLibrary.Inputs;

using Microsoft.Xna.Framework.Input;

namespace WindowsGame1.EngineImplementation
{
    public class XnaInputContext : InputContext
    {
        public override KeyboardStateBase KeyboardGetState()
        {
            return new XnaKeyboardState(Microsoft.Xna.Framework.Input.Keyboard.GetState());
        }

        private class XnaKeyboardState : KeyboardStateBase
        {
            private KeyboardState states;

            public XnaKeyboardState(KeyboardState states)
            {
                this.states = states;
            }

            public override bool IsKeyDown(KeyboardKeys key)
            {
                return this.states.IsKeyDown(ConvertToXnaKey(key));
            }

            private static Keys ConvertToXnaKey(KeyboardKeys key)
            {
                switch (key)
                {
                    case KeyboardKeys.Left: return Keys.Left;
                    case KeyboardKeys.Right: return Keys.Right;
                    case KeyboardKeys.Up: return Keys.Up;
                    case KeyboardKeys.Down: return Keys.Down;
                    case KeyboardKeys.Q: return Keys.Q;
                    case KeyboardKeys.W: return Keys.W;
                    case KeyboardKeys.A: return Keys.A;
                    case KeyboardKeys.Z: return Keys.Z;
                    default: throw new NotSupportedException("Key not supported yet");
                }
            }
        }
    }
}