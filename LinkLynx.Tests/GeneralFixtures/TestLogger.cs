using LinkLynx.Core.Interfaces.Utility.Debugging.Logging;

namespace LinkLynx.Tests.GeneralFixtures
{
    internal class TestLogger : ILogger
    {
        public TestLogger() { }

        public List<string> Messages { get; } = new List<string>();

        public void Log(string message)
        {
            //TestContext.WriteLine(message);
            Messages.Add(message);
        }
    }
}
