using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using GameFramework;

namespace GameNavigator.Navigator
{
    public class NavigatorNode : ViewModelBase
    {
        private readonly ObservableCollection<NavigatorNode> nodeCollection;

        private bool isSelected;
        private bool isExpanded;

        public NavigatorNode(string label, IEnumerable<NavigatorNode> nodes = null)
            : this(null, label, NodeKind.Unknown, nodes)
        {
        }

        public NavigatorNode(object sceneNode, string label, IEnumerable<NavigatorNode> nodes = null)
            : this(sceneNode, label, NodeKind.Unknown, nodes)
        {
        }

        public NavigatorNode(object sceneNode, string label, NodeKind kind, IEnumerable<NavigatorNode> nodes = null)
        {
            this.SceneNode = sceneNode;
            this.Label = label;
            this.Kind = kind;

            this.nodeCollection = new ObservableCollection<NavigatorNode>();
            if (nodes != null) foreach (var node in nodes) this.nodeCollection.Add(node);

            this.Nodes = CollectionViewSource.GetDefaultView(this.nodeCollection);
        }

        public ICollectionView Nodes { get; private set; }

        public int NodeCount
        {
            get { return this.nodeCollection.Count; }
        }

        public object SceneNode { get; private set; }

        public string Label { get; private set; }

        public NodeKind Kind { get; set; }

        public string Icon
        {
            get
            {
                switch (this.Kind)
                {
                    case NodeKind.ScreenState:
                        return "../Icons/ScreenState.png";
                    case NodeKind.Screen:
                        return "../Icons/Screen.png";
                    case NodeKind.Scene:
                        return "../Icons/Scene.png";
                    case NodeKind.Layer:
                        return "../Icons/Layer.png";
                    case NodeKind.Entity:
                        return "../Icons/Entity.png";
                    case NodeKind.Utility:
                        return "../Icons/Utility.png";
                    case NodeKind.Unknown:
                        return "../Icons/Unknown.png";
                    default:
                        throw new InvalidOperationException(this.Kind + " not yet implemented");
                }
            }
        }

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

        public bool IsExpanded
        {
            get
            {
                return this.isExpanded;
            }

            set
            {
                if (this.isExpanded != value)
                {
                    this.isExpanded = value;
                    this.OnPropertyChanged();
                }
            }
        }

        public void UpdateNodes(IEnumerable<NavigatorNode> nodes)
        {
            var nodeList = nodes.ToList();

            var newNodes = nodeList.Except(this.nodeCollection, new NavigatorNodeEqualityComparer());
            var extraNodes = this.nodeCollection.Except(nodeList, new NavigatorNodeEqualityComparer());

            foreach (var extraNode in extraNodes) this.nodeCollection.Remove(extraNode);
            foreach (var newNode in newNodes) this.nodeCollection.Add(newNode);
        }

        private sealed class NavigatorNodeEqualityComparer : IEqualityComparer<NavigatorNode>
        {
            public bool Equals(NavigatorNode x, NavigatorNode y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (ReferenceEquals(x, null)) return false;
                if (ReferenceEquals(y, null)) return false;
                return x.GetType() == y.GetType() && object.Equals(x.SceneNode, y.SceneNode);
            }

            public int GetHashCode(NavigatorNode obj)
            {
                return obj.SceneNode != null ? obj.SceneNode.GetHashCode() : 0;
            }
        }
    }
}