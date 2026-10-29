namespace LinkLynx.Nodes.Model.Connections
{
    public class Edge
    {
        public int SourceNodeId { get; init; }
        public int TargetNodeId { get; init; }

        public string SourcePortId { get; init; }
        public string TargetPortId { get; init; }

        public Edge(int sourceNodeId, int targetNodeId, string sourcePortId, string targetPortid)
        {
            SourceNodeId = sourceNodeId;
            TargetNodeId = targetNodeId;

            SourcePortId = sourcePortId;
            TargetPortId = targetPortid;
        }
    }
}
