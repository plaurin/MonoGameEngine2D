using System;

namespace GameFramework.Sheets
{
    public abstract class SheetBase : INavigatorMetadataProvider
    {
        protected SheetBase(Texture texture, string name)
        {
            this.Texture = texture;
            this.Name = name;
        }

        public Texture Texture { get; private set; }

        public string Name { get; private set; }

        public NavigatorMetadata GetMetadata()
        {
            // TODO need Kind and Icon for Sheets
            return new NavigatorMetadata("Sheet - " + this.Name, NodeKind.Utility);
        }
    }
}