namespace LinkLynx.Wiring.DI
{
    /// <summary>
    /// Specifies the lifetime of a service in a dependency injection container.
    /// </summary>
    /// <remarks>The <see cref="ServiceLifetime"/> enumeration defines how instances of a service are managed
    /// within the container. The lifetime determines whether the same instance is reused or a new instance is created
    /// for each request.</remarks>
    public enum ServiceLifetime
    {
        /// <summary>
        /// Represents a design pattern that ensures a class has only one instance  and provides a global point of
        /// access to it.
        /// </summary>
        /// <remarks>The Singleton pattern is commonly used to manage shared resources, such as a
        /// configuration object  or a connection pool, ensuring that only one instance of the class exists throughout
        /// the application's lifecycle.</remarks>
        Singleton,

        /// <summary>
        /// Specifies that the associated service has a transient lifetime. Transient services are created each time
        /// they are requested. This is typically used for lightweight, stateless services.
        /// </summary>
        /// <remarks>Transient lifetime is suitable for services that do not maintain state and are
        /// inexpensive to instantiate. Each dependency injection request for a transient service results in a new
        /// instance being created.</remarks>
        Transient 
    }
}
