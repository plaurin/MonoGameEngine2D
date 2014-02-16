using System.Collections.Generic;

namespace GameNavigator
{
    public class DesignTimeData
    {
        public static List<NavigatorNode> GetNodes()
        {
            return new List<NavigatorNode>
            {
                new NavigatorNode("A"),
                new NavigatorNode("B", new[]
                {
                    new NavigatorNode("1"),
                    new NavigatorNode("2")
                }),
                new NavigatorNode("C"),
                new NavigatorNode("D", new[]
                {
                    new NavigatorNode("3"),
                    new NavigatorNode("4")
                }),
                new NavigatorNode("E")
            };
        }
    }
}