namespace Crisp.Interfaces.Configuration
{
    /// <summary>
    /// Represents a Crisp configuration file name provider service.
    /// </summary>
    public interface ICrispConfigurationFileNameProvider
    {
        /// <summary>
        /// Gets the name of the configuration file on-disk.
        /// </summary>
        /// <returns></returns>
        string Get();
    }
}