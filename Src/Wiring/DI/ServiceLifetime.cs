namespace LinkLynx.Wiring.DI
{
    public enum ServiceLifetime
    {
        Singleton, // one shared instance
        Transient  // new instance each time
        // Scoped could be added later if you ever need it
    }
}
