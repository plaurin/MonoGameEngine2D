using System;

using GameFramework;
using GameFramework.Inputs;

using Microsoft.Xna.Framework.Input;

namespace MonoGameImplementation.EngineImplementation
{
    public class XnaInputContext : InputContext
    {
        public override KeyboardStateBase KeyboardGetState()
        {
            return new XnaKeyboardState(Microsoft.Xna.Framework.Input.Keyboard.GetState());
        }

        public override MouseStateBase MouseGetState()
        {
            return new XnaMouseState(Mouse.GetState());
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
                    case KeyboardKeys.Enter: return Keys.Enter;
                    case KeyboardKeys.Back: return Keys.Back;
                    case KeyboardKeys.Space: return Keys.Space;
                    default: throw new NotSupportedException("Key not supported yet");
                }
            }
        }

        private class XnaMouseState : MouseStateBase
        {
            private readonly MouseState mouseState;

            public XnaMouseState(MouseState mouseState)
            {
                this.mouseState = mouseState;
            }

            public override bool IsButtonDown(MouseButtons button)
            {
                switch (button)
                {
                    case MouseButtons.Left:
                        return this.mouseState.LeftButton == ButtonState.Pressed;
                    case MouseButtons.Middle:
                        return this.mouseState.MiddleButton == ButtonState.Pressed;
                    case MouseButtons.Right:
                        return this.mouseState.RightButton == ButtonState.Pressed;
                    default: 
                        throw new NotSupportedException("Button not supported yet");
                }
            }

            public override Point Position
            {
                get
                {
                    return new Point(this.mouseState.X, this.mouseState.Y);
                }
            }
        }
    }
}