namespace Crisp.Interfaces
{
    /// <summary>
    /// Represents a raw configuration provider service.
    /// </summary>

    public interface IRawCrispConfigurationProvider
    {
        /// <summary>
        /// Gets the plain text of the configuration file on-disk.
        /// </summary>
        /// <returns></returns>
        string Get();
    }
}
