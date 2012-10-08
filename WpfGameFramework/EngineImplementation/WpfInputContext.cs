using System;
using System.Collections.Generic;
using System.Windows.Input;

using GameFramework;
using GameFramework.Inputs;

namespace WpfGameFramework.EngineImplementation
{
    public class WpfInputContext : InputContext
    {
        private readonly HashSet<Key> keys;

        private readonly Dictionary<MouseButton, MouseButtonState> buttons;

        private readonly Point mousePosition;

        public WpfInputContext(HashSet<Key> keys, Dictionary<MouseButton, MouseButtonState> buttons, Point mousePosition)
        {
            this.keys = keys;
            this.buttons = buttons;
            this.mousePosition = mousePosition;
        }

        public override KeyboardStateBase KeyboardGetState()
        {
            return new WinKeyboardState(this.keys);
        }

        public override MouseStateBase MouseGetState()
        {
            return new WinMouseState(this.buttons, this.mousePosition);
        }

        public class WinKeyboardState : KeyboardStateBase
        {
            private readonly HashSet<Key> keys;

            public WinKeyboardState(HashSet<Key> keys)
            {
                this.keys = keys;
            }

            public override bool IsKeyDown(KeyboardKeys key)
            {
                var winKey = ConvertToWinKey(key);
                return this.keys.Contains(winKey);
            }

            private static Key ConvertToWinKey(KeyboardKeys key)
            {
                switch (key)
                {
                    case KeyboardKeys.Left: return Key.Left;
                    case KeyboardKeys.Right: return Key.Right;
                    case KeyboardKeys.Up: return Key.Up;
                    case KeyboardKeys.Down: return Key.Down;
                    case KeyboardKeys.Q: return Key.Q;
                    case KeyboardKeys.W: return Key.W;
                    case KeyboardKeys.A: return Key.A;
                    case KeyboardKeys.Z: return Key.Z;
                    default: throw new NotSupportedException("Key not supported yet");
                }
            }
        }

        public class WinMouseState : MouseStateBase
        {
            private readonly Dictionary<MouseButton, MouseButtonState> buttons;

            private readonly Point mousePosition;

            public WinMouseState(Dictionary<MouseButton, MouseButtonState> buttons, Point mousePosition)
            {
                this.buttons = buttons;
                this.mousePosition = mousePosition;
            }

            public override bool IsButtonDown(MouseButtons button)
            {
                switch (button)
                {
                    case MouseButtons.Left:
                        return this.buttons[MouseButton.Left] == MouseButtonState.Pressed;
                    case MouseButtons.Middle:
                        return this.buttons[MouseButton.Middle] == MouseButtonState.Pressed;
                    case MouseButtons.Right:
                        return this.buttons[MouseButton.Right] == MouseButtonState.Pressed;
                    default:
                        throw new NotSupportedException("Button not supported yet");
                }
            }

            public override Point Position
            {
                get
                {
                    return this.mousePosition;
                }
            }
        }
    }
}