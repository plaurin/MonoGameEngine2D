using System;

namespace GameFramework.Drawing
{
    public class DrawingFont : INavigatorMetadataProvider
    {
        public string Name { get; set; }

        public virtual Vector MeasureString(string text)
        {
            throw new NotImplementedException();
        }

        public NavigatorMetadata GetMetadata()
        {
            // TODO need Kind and Icon for Font
            return new NavigatorMetadata("Font - " + this.Name, NodeKind.Utility);
        }
    }
}