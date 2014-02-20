namespace GameFramework
{
    public interface IDrawable
    {
        int Draw(IDrawContext drawContext, Transform transform);
    }
}