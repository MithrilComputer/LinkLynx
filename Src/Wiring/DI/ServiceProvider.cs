using System;
using System.Collections.Generic;
using System.Reflection;

namespace LinkLynx.Wiring.DI
{
    /// <summary>
    /// This class is a simple service provider for resolving dependencies.
    /// </summary>
    public sealed class ServiceProvider : IDisposable
    {
        private readonly Dictionary<Type, ServiceDescriptor> descriptorMap = new Dictionary<Type, ServiceDescriptor>();
        
        private readonly List<IDisposable> toDispose = new List<IDisposable>();

        private bool disposed = false;

        /// <summary>
        /// Creates a new instance of the <see cref="ServiceProvider"/> class with the specified service descriptors.
        /// </summary>
        /// <param name="services"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public ServiceProvider(IEnumerable<ServiceDescriptor> services)
        {
            if (services == null)
                throw new ArgumentNullException("[ServiceProvider] Warning: Cant take in null services");

            foreach (ServiceDescriptor service in services)
            {
                if (service == null)
                    continue;

                descriptorMap[service.ServiceType] = service;
            }
        }

        /// <summary>
        /// Gets the required service of type T. Throws an exception if the service is not registered.
        /// </summary>
        public T GetRequired<T>()
        {
            object service = GetRequired(typeof(T));

            return (T)service;
        }

        /// <summary>
        /// Gets the required service of the specified type. Throws an exception if the service is not registered.
        /// </summary>
        public object GetRequired(Type serviceType)
        {
            if (serviceType == null)
                throw new ArgumentNullException("[ServiceProvider] Warning: Cant take in null serviceType");

            ServiceDescriptor descriptor;

            if (!descriptorMap.TryGetValue(serviceType, out descriptor))
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

        /// <summary>
        /// Creates an instance of the service described by the given descriptor.
        /// </summary>
        public object CreateInstanceFromDescriptor(ServiceDescriptor descriptor)
        {
            if (descriptor == null)
                throw new ArgumentNullException("[ServiceProvider] Error: Cant take in null descriptor");

            if (descriptor.Factory != null)
            {
                object built = descriptor.Factory(this);

                return built;
            }

            Type implementationType = descriptor.ImplementationType != null ? descriptor.ImplementationType : descriptor.ServiceType;

            if (implementationType == null)
                throw new InvalidOperationException("[ServiceProvider] Error: Descriptor has no implementation or service type.");

            ConstructorInfo[] constructors = implementationType.GetConstructors(BindingFlags.Public | BindingFlags.Instance);

            if (constructors == null || constructors.Length == 0)
                throw new InvalidOperationException($"[ServiceProvider] Error: No public constructors found for type '{implementationType}'.");

            ConstructorInfo best = constructors[0];

            for (int i = 1; i < constructors.Length; i++)
            {
                if (constructors[i].GetParameters().Length > best.GetParameters().Length)
                {
                    best = constructors[i];
                }
            }

            ParameterInfo[] parameters = best.GetParameters();

            object[] args = new object[parameters.Length];

            for (int i = 0; i < parameters.Length; i++)
            {
                Type dependencyType = parameters[i].ParameterType;
                args[i] = GetRequired(dependencyType);
            }

            object instance = Activator.CreateInstance(implementationType, args);

            return TrackDisposable(instance);
        }

        /// <summary>
        /// Tracks the disposable instance for later disposal.
        /// </summary>
        private object TrackDisposable(object instance)
        {
            IDisposable disposable = instance as IDisposable;

            if (disposable != null)
            {
                toDispose.Add(disposable);
            }

            return instance;
        }

        /// <summary>
        /// Disposes the service provider and all tracked disposable instances.
        /// </summary>
        public void Dispose()
        {
            if (disposed)
                return;

            disposed = true;

            for (int i = toDispose.Count - 1; i >= 0; i--)
            {
                try
                {
                    toDispose[i].Dispose();
                }
                catch
                { /*swallow exceptions*/ }
            }

            toDispose.Clear();
        }
    }
}
