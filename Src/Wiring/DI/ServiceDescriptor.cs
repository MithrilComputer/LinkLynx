using LinkLynx.Wiring.DI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkLynx.Core.Src.Wiring.DI
{
    public class ServiceDescriptor
    {
        public Type ServiceType { get; }
        public Type ImplementationType { get; }
        public Func<ServiceProvider, object> Factory { get; }
        public ServiceLifetime Lifetime { get; }


        internal object SingletonInstance; // for singleton instances

        public ServiceDescriptor(Type service, Type implementation, ServiceLifetime lifetime)
        {
            Lifetime = lifetime;
            ServiceType = service;
            ImplementationType = implementation;
        }

        public ServiceDescriptor(Type service, Func<ServiceProvider, object> factory, ServiceLifetime lifetime)
        {
            Lifetime = lifetime;
            ServiceType = service;
            Factory = factory;
        }
    }
}
