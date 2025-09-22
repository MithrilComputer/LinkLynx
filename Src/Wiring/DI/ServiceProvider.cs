using System;
using System.Collections.Generic;
using System.Reflection;

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
                    descriptor.SingletonInstance = CreateInstanceFromDescriptor(descriptor);
                }
                return descriptor.SingletonInstance;
            }
            else // Transient
            {
                return CreateInstanceFromDescriptor(descriptor);
            }
        }

        public object CreateInstanceFromDescriptor(ServiceDescriptor descriptor)
        {
            if (descriptor == null)
                throw new ArgumentNullException("[ServiceProvider] Warning: Cant take in null descriptor");

            if (descriptor.Factory != null)
            {
                object built = descriptor.Factory(this);

                return built;
            }

            Type implType = descriptor.ImplementationType != null ? descriptor.ImplementationType : descriptor.ServiceType;

            if (implType == null)
                throw new InvalidOperationException("Descriptor has no implementation or service type.");

            ConstructorInfo[] constructors = implType.GetConstructors(BindingFlags.Public | BindingFlags.Instance);

            if(constructors == null || constructors.Length == 0)
                throw new InvalidOperationException($"No public constructors found for type '{implType}'.");


        }

        private object TrackDisposable(object instance)
        {
            IDisposable disposable = instance as IDisposable;

            if(disposable != null)
            {
                toDispose.Add(disposable);
            }

            return instance;
        }
    }
}
