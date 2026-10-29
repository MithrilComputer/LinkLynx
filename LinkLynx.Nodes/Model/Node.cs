using LinkLynx.Nodes.Model.Ports;

namespace LinkLynx.Nodes.Model
{
    public abstract class Node
    {
        public int Id { get; internal set; }

        private readonly List<Port> ports = new();

        public IReadOnlyList<Port> Ports => ports;

        protected void AddPort(Port port)
        {
            ports.Add(port);
        }
    }
}
