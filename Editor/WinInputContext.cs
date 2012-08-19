using System;
using System.Collections.Generic;
using System.Windows.Input;

using ClassLibrary.Inputs;

namespace Editor
{
    public class WinInputContext : InputContext
    {
        private readonly HashSet<Key> keys;

        public WinInputContext(HashSet<Key> keys)
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

            public override bool IsKeyDown(Keys key)
            {
                var winKey = ConvertToWinKey(key);

                return this.keys.Contains(winKey);
            }

            private static Key ConvertToWinKey(Keys key)
            {
                switch (key)
                {
                    case Keys.Left: return Key.Left;
                    case Keys.Right: return Key.Right;
                    case Keys.Up: return Key.Up;
                    case Keys.Down: return Key.Down;
                    case Keys.Q: return Key.Q;
                    case Keys.W: return Key.W;
                    case Keys.A: return Key.A;
                    case Keys.Z: return Key.Z;
                    default: throw new NotSupportedException("Key not supported yet");
                }
            }
        }
    }
}