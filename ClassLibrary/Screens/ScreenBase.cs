﻿using System;

using ClassLibrary.Cameras;
using ClassLibrary.Inputs;
using ClassLibrary.Scenes;

namespace ClassLibrary.Screens
{
    public abstract class ScreenBase
    {
        public abstract void Initialize(Camera camera);

        public abstract InputConfiguration GetInputConfiguration();

        public abstract void LoadContent(GameResourceManager resourceManager);

        public abstract void Update(double elapsedSeconds, int fps);

        public abstract Scene GetScene();
    }
}