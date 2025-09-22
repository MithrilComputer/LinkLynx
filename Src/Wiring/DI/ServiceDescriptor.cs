using LinkLynx.Wiring.DI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkLynx.Wiring.DI
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

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceDescriptor"/> class with the specified service type,
        /// implementation type, and service lifetime.
        /// </summary>
        /// <param name="service">The type of the service being described. This is the type that will be requested from the service container.</param>
        /// <param name="implementation">The type that implements the service. This type will be instantiated by the service container to fulfill
        /// requests for the service.</param>
        /// <param name="lifetime">The lifetime of the service, which determines how the service is managed by the container. For example, <see
        /// cref="ServiceLifetime.Singleton"/> indicates a single instance is used, while <see
        /// cref="ServiceLifetime.Transient"/> creates a new instance for each request.</param>
        public ServiceDescriptor(Type service, Type implementation, ServiceLifetime lifetime)
        {
            Lifetime = lifetime;
            ServiceType = service;
            ImplementationType = implementation;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceDescriptor"/> class with the specified service type,
        /// factory method, and service lifetime.
        /// </summary>
        /// <param name="service">The type of the service being described. This cannot be <see langword="null"/>.</param>
        /// <param name="factory">A factory method that creates an instance of the service. This cannot be <see langword="null"/>.</param>
        /// <param name="lifetime">The lifetime of the service, which determines how the service is managed by the <see
        /// cref="ServiceProvider"/>.</param>
        public ServiceDescriptor(Type service, Func<ServiceProvider, object> factory, ServiceLifetime lifetime)
        {
            Lifetime = lifetime;
            ServiceType = service;
            Factory = factory;
        }
    }
}
