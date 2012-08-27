using System;
using System.Collections.Generic;
using System.Windows.Input;

using ClassLibrary.Inputs;

namespace WPFGameLibrary.EngineImplementation
{
    public class WpfInputContext : InputContext
    {
        private readonly HashSet<Key> keys;

        public WpfInputContext(HashSet<Key> keys)
        {
            this.keys = keys;
        }

        public override KeyboardStateBase KeyboardGetState()
        {
            return new WinKeyboardState(this.keys);
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
    }
}