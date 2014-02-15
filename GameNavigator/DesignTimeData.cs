using System.Collections.Generic;

namespace GameNavigator
{
    public class DesignTimeData
    {
        public static List<NavigatorNode> GetNodes()
        {
            return new List<NavigatorNode>
            {
                new NavigatorNode { Label = "A" },
                new NavigatorNode 
                { 
                    Label = "B", 
                    Nodes = new[]
                    {
                        new NavigatorNode { Label = "1" },
                        new NavigatorNode { Label = "2" }
                    }
                },
                new NavigatorNode { Label = "C" },
                new NavigatorNode 
                { 
                    Label = "D", 
                    Nodes = new[]
                    {
                        new NavigatorNode { Label = "3" },
                        new NavigatorNode { Label = "4" }
                    }
                },
                new NavigatorNode { Label = "E" }
            };
        }
    }
}