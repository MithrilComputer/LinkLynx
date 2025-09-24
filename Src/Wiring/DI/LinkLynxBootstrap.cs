using Independentsoft.Exchange;
using LinkLynx.Core.Interfaces;
using LinkLynx.Core.Utility.Debugging.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkLynx.Wiring.DI
{
    public static class LinkLynxBootstrap
    {
        public static ServiceProvider CreateDefault()
        {
            ServiceCollection services = new ServiceCollection();

            services.AddSingleton<ILogger, ConsoleLogger>();
        }
    }
}
