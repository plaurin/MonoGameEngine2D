using System;

using WPFGameLibrary.EngineImplementation;

namespace Editor
{
    public class ServiceLocator
    {
        static ServiceLocator()
        {
            GameResourceManager = new WpfGameResourceManager();
        }

        public static WpfGameResourceManager GameResourceManager { get; private set; }
    }
}