using QuickGraph;

namespace Chapter05.Q44.Models
{
    /// <summary>
    /// This is our custom data graph derived from BidirectionalGraph class using custom data types.
    /// Data graph stores vertices and edges data that is used by GraphArea and end-user for a variety of operations.
    /// Data graph content handled manually by user (add/remove objects). The main idea is that you can dynamicaly
    /// remove/add objects into the GraphArea layout and then use data graph to restore original layout content.
    /// </summary>
    public class Graph : BidirectionalGraph<DataVertex, DataEdge> { }
}