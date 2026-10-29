using LinkLynx.Nodes.Model;
using LinkLynx.Nodes.Model.Connections;

namespace LinkLynx.Nodes.Collections
{
    internal class Graph
    {
        public List<Node> Nodes { get; } = new List<Node>();
        public List<Edge> Edges { get; } = new List<Edge>();

        Dictionary<(int NodeId, string PortId), List<Edge>> outgoingEdges = new Dictionary<(int NodeId, string PortId), List<Edge>>();



    }
}
