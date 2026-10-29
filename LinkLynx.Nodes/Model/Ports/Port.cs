namespace LinkLynx.Nodes.Model.Ports
{
    public class Port
    {
        public string Id { get; }

        public PortDirection Direction { get; }

        public Port(string id, PortDirection direction)
        {
            id = Id;
            direction = Direction;
        }
    }
}
