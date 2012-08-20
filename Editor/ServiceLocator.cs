using System;

using ClassLibrary;

namespace Editor
{
    public class ServiceLocator
    {
        static ServiceLocator()
        {
            GameResourceManager = new GameResourceManager();
        }

        public static GameResourceManager GameResourceManager { get; private set; }
    }
}