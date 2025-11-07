using LinkLynx.Core.Interfaces.Utility.Debugging.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkLynx.Tests.SystemTests.Fixtures
{
    internal class TestLogger : ILogger
    {
        public void Log(string message)
        {
            Console.WriteLine(message);
        }
    }
}
