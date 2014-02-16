namespace GameFramework.Sprites
{
    public interface ISpriteTemplate<out T>
        where T : SpriteBase
    {
        T CreateInstance();
    }
}