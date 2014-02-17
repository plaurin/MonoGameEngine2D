using System;
using System.Windows;
using System.Windows.Controls;

namespace GameNavigator.Navigator
{
    public partial class NavigatorView
    {
        public NavigatorView()
        {
            this.InitializeComponent();
        }

        private void TreeViewItemExpandedCollapsed(object sender, RoutedEventArgs e)
        {
            var treeViewItem = sender as TreeViewItem;
            if (treeViewItem != null)
            {
                var navigatorNode = treeViewItem.DataContext as NavigatorNode;
                if (navigatorNode != null)
                    navigatorNode.IsExpanded = treeViewItem.IsExpanded;
            }
        }
    }
}
