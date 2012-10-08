using System;

using WpfGameFramework.EngineImplementation;

namespace WpfGameLoader
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