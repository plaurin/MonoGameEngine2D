using System;

using ShootEmUpGameDomain;

using XnaGameFramework;

namespace XnaShootEmUpGame
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game : WindowsGameBase
    {
        public Game()
            : base(new DefaultScreen())
        {
            IsMouseVisible = true;
        }
    }
}
