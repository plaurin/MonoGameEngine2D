using System.Collections.Generic;

namespace GameNavigator
{
    public class NavigatorNode
    {
        public IEnumerable<NavigatorNode> Nodes { get; set; }

        public string Label { get; set; }
    }
}