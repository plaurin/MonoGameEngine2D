using System;

using ClassLibrary;
using ClassLibrary.Hexes;

namespace Editor
{
    public class WinHexDefinition : HexDefinition
    {
        public WinHexDefinition(HexSheet sheet, string name, Rectangle rectangle)
            : base(sheet, name, rectangle)
        {
        }
    }
}