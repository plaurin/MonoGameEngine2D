using System;

namespace GameFramework.Tiles
{
    public class TileDefinition : INavigatorMetadataProvider
    {
        public TileDefinition(TileSheet sheet, string name, RectangleInt rectangle)
        {
            this.Sheet = sheet;
            this.Name = name;
            this.Rectangle = rectangle;
        }

        public TileSheet Sheet { get; private set; }

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
            // TODO need kind and icon for TileDefinition (any kind of definition/template?)
            return new NavigatorMetadata(this.Name);
        }
    }
}