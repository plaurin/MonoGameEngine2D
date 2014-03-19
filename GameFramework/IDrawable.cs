namespace GameFramework
{
    public interface IDrawable
    {
        int Draw(IDrawContext drawContext, Transform transform);
    }

    public interface IPreDrawable
    {
        void PreDraw(IDrawContext drawContext);
    }
}