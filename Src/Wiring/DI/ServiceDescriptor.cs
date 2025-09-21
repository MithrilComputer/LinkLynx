using LinkLynx.Wiring.DI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkLynx.Core.Src.Wiring.DI
{
    /// <summary>
    /// This class describes a service registration in the DI container.
    /// </summary>
    public class ServiceDescriptor
    {
        /// <summary>
        /// Gets the type of the service associated with this instance.
        /// </summary>
        public Type ServiceType { get; }

        /// <summary>
        /// Gets the <see cref="Type"/> that represents the implementation type associated with this instance.
        /// </summary>
        public Type ImplementationType { get; }

        /// <summary>
        /// A factory function to create the service.
        /// </summary>
        public Func<ServiceProvider, object> Factory { get; }

        /// <summary>
        /// Gets the lifetime of the service in the dependency injection container.
        /// </summary>
        public ServiceLifetime Lifetime { get; }

        /// <summary>
        /// Represents the singleton instance of the associated type.
        /// </summary>
        /// <remarks>This field is intended for internal use to store a single, shared instance of the
        /// type. It is not thread-safe and should be accessed with appropriate synchronization if used in a
        /// multithreaded context.</remarks>
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
