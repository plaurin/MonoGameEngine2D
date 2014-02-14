using System;
using GameFramework.Inputs;

namespace GameFramework.Screens
{
    public abstract class ScreenBase
    {
        public abstract void Initialize(Viewport viewport);

        public abstract void LoadContent(GameResourceManager theResourceManager);

        public abstract void Update(InputContext inputContext, IGameTiming gameTime);

        public abstract int Draw(DrawContext drawContext);
    }
}