using Crestron.SimplSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkLynx.Core.Utility.Debugging.Logging
{
    public static class ConsoleLogger
    {
        public static void Log(string message) => CrestronConsole.PrintLine(message);
    }
}
