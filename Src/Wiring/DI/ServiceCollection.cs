using LinkLynx.Wiring.DI;
using System;

namespace LinkLynx.Core.Src.Wiring.DI
{
    public sealed class ServiceCollection
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
    }
}
