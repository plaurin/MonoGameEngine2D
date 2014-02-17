using System;

namespace GameFramework
{
    public abstract class Texture : INavigatorMetadataProvider
    {
        public string Name { get; set; }

        public NavigatorMetadata GetMetadata()
        {
            // TODO Need kind and icon for Texture
            return new NavigatorMetadata("Texture - " + this.Name, NodeKind.Utility);
        }
    }
}