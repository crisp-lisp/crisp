namespace Packet.Configuration
{
    /// <summary>
    /// Represents an application configuration settings provider.
    /// </summary>
    internal interface IConfigurationProvider
    {
        /// <summary>
        /// Gets the configuration object.
        /// </summary>
        /// <returns></returns>
        Configuration GetConfiguration();
    }
}
