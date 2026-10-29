namespace LinkLynx.Nodes.Model.Ports
{
    public class DataPort<T> : Port
    {
        public T Value { get; internal set; }

        public DataPort(string id, PortDirection portDirection) : base(id, portDirection)
        {

        }
    }
}
