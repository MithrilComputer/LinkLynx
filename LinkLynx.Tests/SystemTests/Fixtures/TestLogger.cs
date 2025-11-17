using LinkLynx.Core.Interfaces.Utility.Debugging.Logging;
using NUnit.Framework;

namespace LinkLynx.Tests.SystemTests.Fixtures
{
    internal class TestLogger : ILogger
    {
        public void Log(string message)
        {
            TestContext.WriteLine(message);
        }
    }
}
