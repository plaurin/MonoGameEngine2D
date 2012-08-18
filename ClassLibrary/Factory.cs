using System;

using ClassLibrary.Maps;

namespace ClassLibrary
{
    public abstract class Factory
    {
        public abstract ColorMap CreateColorMap(string name, Color color);

        public abstract ImageMap CreateImageMap(string name, Texture getTexture, Rectangle parseRectangle);
    }
}