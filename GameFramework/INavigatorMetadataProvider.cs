namespace GameFramework
{
    public interface INavigatorMetadataProvider
    {
        NavigatorMetadata GetMetadata();
    }

    public class NavigatorMetadata
    {
        public NavigatorMetadata(string name, NodeKind kind = NodeKind.Unknown, bool isActive = false)
        {
            this.Name = name;
            this.Kind = kind;
            this.IsActive = isActive;
        }

        public string Name { get; private set; }

        public NodeKind Kind { get; private set; }

        public bool IsActive { get; private set; }
    }

    public enum NodeKind
    {
        ScreenState,
        Screen,
        Scene,
        Camera,
        Layer,
        Entity,
        Utility,
        Unknown,
    }
}