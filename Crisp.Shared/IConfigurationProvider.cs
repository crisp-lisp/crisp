namespace Crisp.Shared
{
    /// <summary>
    /// Represents an application configuration settings provider.
    /// </summary>
    public interface ICrispConfigurationProvider
    {
        /// <summary>
        /// Gets the configuration object.
        /// </summary>
        /// <returns></returns>
        ICrispConfiguration Get();
    }
}
