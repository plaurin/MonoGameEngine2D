using System;
using GameFramework.Inputs;
using GameFramework.Scenes;

namespace GameFramework.Screens
{
    public abstract class ScreenBase
    {
        public abstract void Initialize(Viewport viewport);

        public abstract void LoadContent(GameResourceManager theResourceManager);

        public abstract void Update(InputContext inputContext, IGameTiming gameTime);

        // TODO: Remove (Tell don't ask)
        public abstract Scene GetScene();

        public abstract int Draw(DrawContext drawContext);
    }
}