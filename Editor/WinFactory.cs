using System;

using ClassLibrary;
using ClassLibrary.Maps;

namespace Editor
{
    public class WinFactory : Factory
    {
        public override ColorMap CreateColorMap(string name, Color color)
        {
            return new WinColorMap(name, color);
        }

        public override ImageMap CreateImageMap(string name, Texture texture, Rectangle rectangle)
        {
            return new WinImageMap(name, texture, rectangle);
        }
    }
}