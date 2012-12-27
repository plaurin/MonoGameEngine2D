using System;
using DemoGameDomain;
using XnaGameFramework;

namespace MonoWindowsDemoGame
{
    public class DemoGame : WindowsGameBase
    {
        public DemoGame()
            : base(new DefaultScreen())
        {
            IsMouseVisible = true;
        }
    }
}
