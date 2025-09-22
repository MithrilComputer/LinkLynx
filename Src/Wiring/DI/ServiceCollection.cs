using LinkLynx.Core.Src.Wiring.DI;
using System;
using System.Collections.Generic;

namespace LinkLynx.Wiring.DI
{
    /// <summary>
    /// Represents a collection of service descriptors used for configuring dependency injection.
    /// </summary>
    /// <remarks>The <see cref="ServiceCollection"/> class provides methods to register services with
    /// different lifetimes,  such as singleton and transient, and to build a <see cref="ServiceProvider"/> for
    /// resolving dependencies. This class is typically used in applications to configure and manage service
    /// dependencies.</remarks>
    public sealed class ServiceCollection
    {
        /// <summary>
        /// Represents a collection of service descriptors used to configure services.
        /// </summary>
        /// <remarks>This collection is intended to store <see cref="ServiceDescriptor"/> instances, which
        /// describe the services and their lifetimes for dependency injection. It is typically used internally to
        /// manage service registrations.</remarks>
        private readonly List<ServiceDescriptor> descriptors = new List<ServiceDescriptor>();

        // Public API ---------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Adds a singleton service of the specified type with a specified implementation type to the service
        /// collection.
        /// </summary>
        /// <remarks>A singleton service is created once and shared across the application. The same
        /// instance of the service will be returned for every request.</remarks>
        /// <typeparam name="TService">The type of the service to register.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation to use for the service. Must implement <typeparamref name="TService"/>.</typeparam>
        /// <returns>The current <see cref="ServiceCollection"/> instance, allowing for method chaining.</returns>
        public ServiceCollection AddSingleton<TService, TImplementation>() where TImplementation : TService
        {
            descriptors.Add(new ServiceDescriptor(typeof(TService), typeof(TImplementation), ServiceLifetime.Singleton));
            return this;
        }

        /// <summary>
        /// Adds a singleton service of the specified type to the service collection.
        /// </summary>
        /// <remarks>A singleton service is created once and shared across all requests. The provided
        /// factory function  is invoked only once to create the service instance, and the same instance is returned for
        /// all  subsequent requests for the service.</remarks>
        /// <typeparam name="TService">The type of the service to register.</typeparam>
        /// <param name="factory">A factory function that creates an instance of the service. The function receives the  <see
        /// cref="ServiceProvider"/> as a parameter, allowing access to other registered services.</param>
        /// <returns>The current <see cref="ServiceCollection"/> instance, enabling method chaining.</returns>
        public ServiceCollection AddSingleton<TService>(Func<ServiceProvider, object> factory)
        {
            descriptors.Add(new ServiceDescriptor(typeof(TService), factory, ServiceLifetime.Singleton));
            return this;
        }

        /// <summary>
        /// Adds a pre-created singleton instance of the specified service type to the service collection.
        /// </summary>
        /// <remarks>The provided <paramref name="instance"/> will be used as the single shared instance
        /// for the specified service type. This method is typically used when you already have an instance of the
        /// service and want to ensure it is reused throughout the application's lifetime.</remarks>
        /// <typeparam name="TService">The type of the service to register.</typeparam>
        /// <param name="instance">The instance of the service to register as a singleton. This instance will be returned for all requests for
        /// the service.</param>
        /// <returns>The current <see cref="ServiceCollection"/> instance, allowing for method chaining.</returns>
        public ServiceCollection AddSingleton<TService>(TService instance)
        {

            Func<ServiceProvider, object> factory = delegate (ServiceProvider serviceProvider)
            {
                return instance;
            };

            // Pre created singleton instance
            ServiceDescriptor singletonServiceDescriptor = new ServiceDescriptor(typeof(TService), factory, ServiceLifetime.Singleton)
            {
                // Cache immediately so provider returns this same instance
                SingletonInstance = instance
            };

            descriptors.Add(singletonServiceDescriptor);

            return this;
        }

        /// <summary>
        /// Adds a transient service of the specified type with an implementation type to the service collection.
        /// </summary>
        /// <remarks>Transient services are created each time they are requested. This lifetime is
        /// suitable for lightweight, stateless services.</remarks>
        /// <typeparam name="TService">The type of the service to add.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation to use for the service. Must derive from <typeparamref name="TService"/>.</typeparam>
        /// <returns>The current <see cref="ServiceCollection"/> instance, allowing for method chaining.</returns>
        public ServiceCollection AddTransient<TService, TImplementation>() where TImplementation : TService
        {
            descriptors.Add(new ServiceDescriptor(typeof(TService), typeof(TImplementation), ServiceLifetime.Transient));
            return this;
        }

        /// <summary>
        /// Adds a transient service of the specified type to the service collection.
        /// </summary>
        /// <remarks>Transient services are created each time they are requested. Use this method to
        /// register services  that are lightweight and stateless, or when a new instance is required for each
        /// request.</remarks>
        /// <typeparam name="TService">The type of the service to add.</typeparam>
        /// <param name="factory">A factory function that creates an instance of <typeparamref name="TService"/>. The function takes a  <see
        /// cref="ServiceProvider"/> as a parameter and returns the created service instance.</param>
        /// <returns>The current <see cref="ServiceCollection"/> instance, allowing for method chaining.</returns>
        public ServiceCollection AddTransient<TService>(Func<ServiceProvider, TService> factory)
        {
            object wrapper(ServiceProvider serviceProvider)
            {
                return factory(serviceProvider);
            }

            descriptors.Add(new ServiceDescriptor(typeof(TService), wrapper, ServiceLifetime.Transient));
            return this;
        }

        /// <summary>
        /// Builds and returns a new <see cref="ServiceProvider"/> instance configured with the current service
        /// descriptors.
        /// </summary>
        /// <remarks>The returned <see cref="ServiceProvider"/> resolves services based on the descriptors
        /// added to this builder. Ensure all required service dependencies are registered before calling this
        /// method.</remarks>
        /// <returns>A <see cref="ServiceProvider"/> instance that can be used to resolve services.</returns>
        public ServiceProvider BuildServiceProvider()
        {
            return new ServiceProvider(descriptors);
        }

        // Internals ---------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Adds a service descriptor to the collection with the specified service type, implementation type, and
        /// lifetime.
        /// </summary>
        /// <param name="service">The type of the service to register.</param>
        /// <param name="implementation">The type that implements the service.</param>
        /// <param name="lifetime">The lifetime of the service, which determines its scope and reuse behavior.</param>
        /// <returns>The current <see cref="ServiceCollection"/> instance, allowing for method chaining.</returns>
        private ServiceCollection Add(Type service, Type implementation, ServiceLifetime lifetime)
        {
            descriptors.Add(new ServiceDescriptor(service, implementation, lifetime));
            return this;
        }

        /// <summary>
        /// Adds a service descriptor to the collection with the specified service type, factory, and lifetime.
        /// </summary>
        /// <param name="service">The <see cref="Type"/> of the service to add.</param>
        /// <param name="factory">A factory function that creates an instance of the service. The function receives a <see
        /// cref="ServiceProvider"/> as an argument.</param>
        /// <param name="lifetime">The <see cref="ServiceLifetime"/> that specifies the lifetime of the service.</param>
        /// <returns>The current <see cref="ServiceCollection"/> instance, allowing for method chaining.</returns>
        private ServiceCollection Add(Type service, Func<ServiceProvider, object> factory, ServiceLifetime lifetime)
        {
            descriptors.Add(new ServiceDescriptor(service, factory, lifetime));
            return this;
        }
    }
}
