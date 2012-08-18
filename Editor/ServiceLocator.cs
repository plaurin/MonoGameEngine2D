using System;

using ClassLibrary;

namespace Editor
{
    public class ServiceLocator
    {
        static ServiceLocator()
        {
            GameResourceManager = new GameResourceManager();
            Factory = new WinFactory();
        }

        public static GameResourceManager GameResourceManager { get; private set; }

        public static Factory Factory { get; private set; }
    }
}