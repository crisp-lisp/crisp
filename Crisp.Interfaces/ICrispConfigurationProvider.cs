namespace Crisp.Interfaces
{
    /// <summary>
    /// Represents an application configuration settings provider service.
    /// </summary>
    public interface ICrispConfigurationProvider
    {
        /// <summary>
        /// Gets the application configuration settings.
        /// </summary>
        /// <returns></returns>
        ICrispConfiguration Get();
    }
}
