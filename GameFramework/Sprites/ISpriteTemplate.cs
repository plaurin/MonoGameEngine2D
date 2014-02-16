using System;

namespace GameFramework.Sprites
{
    public interface ISpriteTemplate
    {
        SpriteBase CreateInstance();
    }
}