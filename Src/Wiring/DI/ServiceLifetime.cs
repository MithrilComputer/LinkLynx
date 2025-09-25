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
        Singleton, // one shared instance
        Transient  // new instance each time
    }
}
