using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;

namespace GameNavigator.Navigator
{
    public class NavigatorNode : ViewModelBase
    {
        private readonly ObservableCollection<NavigatorNode> nodeCollection;

        private bool isSelected;

        public NavigatorNode(string label, IEnumerable<NavigatorNode> nodes = null)
            : this(null, label, nodes)
        {
        }

        public NavigatorNode(object sceneNode, string label, IEnumerable<NavigatorNode> nodes = null, string icon = "")
        {
            this.SceneNode = sceneNode;
            this.Label = label;
            this.Icon = icon;

            this.nodeCollection = new ObservableCollection<NavigatorNode>();
            if (nodes != null) foreach (var node in nodes) this.nodeCollection.Add(node);

            this.Nodes = CollectionViewSource.GetDefaultView(this.nodeCollection);
        }

        public ICollectionView Nodes { get; private set; }

        public object SceneNode { get; private set; }

        public string Label { get; private set; }

        public string Icon { get; private set; }

        public bool IsSelected
        {
            get
            {
                return this.isSelected;
            }

            set
            {
                if (this.isSelected != value)
                {
                    this.isSelected = value;
                    this.OnPropertyChanged();
                }
            }
        }

        public void UpdateNodes(IEnumerable<NavigatorNode> nodes)
        {
            this.nodeCollection.Clear();
            foreach (var node in nodes) this.nodeCollection.Add(node);
        }
    }
}