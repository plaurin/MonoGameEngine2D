using System;

namespace GameFramework.Hexes
{
    public class HexDefinition : INavigatorMetadataProvider
    {
        public HexDefinition(HexSheet sheet, string name, RectangleInt rectangle)
        {
            this.Sheet = sheet;
            this.Name = name;
            this.Rectangle = rectangle;
        }

        public HexSheet Sheet { get; private set; }

        public string SheetName
        {
            get { return this.Sheet.Name; }
        }
        
        public string Name { get; private set; }

        public RectangleInt Rectangle { get; private set; }

        public virtual bool ShouldDraw
        {
            get { return true; }
        }

        public virtual void Draw(IDrawContext drawContext, Rectangle destination)
        {
            this.Sheet.Draw(drawContext, this, destination);
        }

        public NavigatorMetadata GetMetadata()
        {
            return new NavigatorMetadata(this.Name);
        }
    }
}