using System;

using ClassLibrary;

namespace Editor
{
    public class WinTexture : Texture
    {
        public WinTexture(string name, string filePath)
        {
            this.FilePath = filePath;
            this.Name = name;
        }

        public string FilePath { get; private set; }
    }
}