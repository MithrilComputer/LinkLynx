using System.Diagnostics;
using System.Reflection;

namespace LinkLynx.Wiring.DI
{
    /// <summary>
    /// This class is a simple service provider for resolving dependencies.
    /// </summary>
    public sealed class ServiceProvider : IDisposable
    {
        private readonly Dictionary<Type, ServiceDescriptor> descriptorMap = new Dictionary<Type, ServiceDescriptor>();

        private readonly HashSet<Type> buildStack = new HashSet<Type>();

        private readonly List<IDisposable> toDispose = new List<IDisposable>();

        private bool disposed = false;

        /// <summary>
        /// Creates a new instance of the <see cref="ServiceProvider"/> class with the specified service descriptors.
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        public ServiceProvider(IEnumerable<ServiceDescriptor> services)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services), "[ServiceProvider] Warning: Cant take in null services");

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
                throw new ArgumentNullException(nameof(serviceType), "[ServiceProvider] Warning: Cant take in null serviceType");

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
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public object CreateInstanceFromDescriptor(ServiceDescriptor descriptor)
        {
            if (descriptor == null)
                throw new ArgumentNullException(nameof(descriptor), "[ServiceProvider] Error: Cant take in null descriptor");

            // Detect circular dependencies, Almost forgot to add this lol
            var implementationType = descriptor.ImplementationType ?? descriptor.ServiceType;
            if (buildStack.Contains(implementationType))
            {
                string chain = string.Join(" -> ", buildStack);
                throw new InvalidOperationException(
                    $"[ServiceProvider] Circular dependency detected: {chain} -> {implementationType}. " +
                    $"Consider refactoring to break constructor dependency cycle (use factory, interface segregation, or mediator).");
            }

            buildStack.Add(implementationType); // push

            try
            {
                // Factory registered? I sure hope so :D
                if (descriptor.Factory != null)
                {
                    object built = descriptor.Factory(this);
                    if (built == null)
                        throw new InvalidOperationException($"[ServiceProvider] Factory returned null for {implementationType}.");

                    return TrackDisposable(built);
                }

                var constructors = implementationType.GetConstructors(BindingFlags.Public | BindingFlags.Instance);
                if (constructors == null || constructors.Length == 0)
                    throw new InvalidOperationException($"[ServiceProvider] No public constructors found for '{implementationType}'.");

                // Use the constructor with the most parameters
                var best = constructors
                    .OrderByDescending(c => c.GetParameters().Length)
                    .First();

                var parameters = best.GetParameters();
                var args = new object[parameters.Length];

                // Validate dependencies exist
                for (int i = 0; i < parameters.Length; i++)
                {
                    var depType = parameters[i].ParameterType;
                    try
                    {
                        args[i] = GetRequired(depType);
                    }
                    catch (InvalidOperationException)
                    {
                        throw new InvalidOperationException(
                            $"[ServiceProvider] Missing dependency '{depType}' while building '{implementationType}'. " +
                            $"Did you forget to register it?");
                    }
                }

                var instance = Activator.CreateInstance(implementationType, args);
                return TrackDisposable(instance);
            }
            finally
            {
                buildStack.Remove(implementationType); // pop
            }
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
                catch (Exception ex)
                {
                    Debug.WriteLine($"Dispose failed: {ex.Message}");
                }
            }

            toDispose.Clear();
        }
    }
}
