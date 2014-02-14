using GameFramework.Inputs;

namespace GameFramework.Screens
{
    public interface IScreen
    {
        bool ShouldExit { get; }

        void Initialize(Viewport viewport);

        void LoadContent(GameResourceManager theResourceManager);

        void Update(InputContext inputContext, IGameTiming gameTime);

        int Draw(DrawContext drawContext);
    }
}