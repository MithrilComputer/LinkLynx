using System;
using System.Collections.Generic;

namespace LinkLynx.Wiring.DI
{
    public sealed class ServiceProvider : IDisposable
    {
        private readonly Dictionary<Type, ServiceDescriptor> discriptorMap = new Dictionary<Type, ServiceDescriptor>();
        private readonly List<IDisposable> toDispose = new List<IDisposable>();
        private bool disposed = false;

        public ServiceProvider(IEnumerable<ServiceDescriptor> services)
        {
            if (services == null)
                throw new ArgumentNullException("[ServiceProvider] Warning: Cant take in null services");

            foreach (ServiceDescriptor service in services)
            {
                if(service == null)
                    continue;

                discriptorMap[service.ServiceType] = service;
            }
        }

        public T GetRequired<T>()
        {
            object service = GetRequired(typeof(T));

            return (T)service;
        }

        public object GetRequired(Type serviceType)
        {
            if (serviceType == null)
                throw new ArgumentNullException("[ServiceProvider] Warning: Cant take in null serviceType");

            ServiceDescriptor descriptor;

            if(!discriptorMap.TryGetValue(serviceType, out descriptor))
            {
                throw new InvalidOperationException($"[ServiceProvider] Error: No service for type '{serviceType}' has been registered.");
            }

            if (descriptor.Lifetime == ServiceLifetime.Singleton)
            {
                if (descriptor.SingletonInstance == null)
                {
                    //_!___!__!_!___!_!_!_!_!!!_______________________!!_!_!_!_
                    // todo Create and cache singleton instance
                    //descriptor.SingletonInstance = CreateServiceInstance(descriptor);
                }
                return descriptor.SingletonInstance;
            }
            else // Transient
            {
                // Todo Create a new instance each time
                //return CreateServiceInstance(descriptor);
            }
        }

        public object CreateInstance(ServiceDescriptor descriptor)
        {
            if(descriptor == null)
                throw new ArgumentNullException("[ServiceProvider] Warning: Cant take in null descriptor");

            if(descriptor.Factory != null)
            {
                object built = descriptor.Factory(this);

                return built;
            }


        }
    }
}
