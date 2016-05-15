namespace Packet.Interfaces.Configuration
{
    /// <summary>
    /// Represents an application configuration settings provider service.
    /// </summary>
    public interface IPacketConfigurationProvider
    {
        /// <summary>
        /// Gets the application configuration settings.
        /// </summary>
        /// <returns></returns>
        IPacketConfiguration Get();
    }
}
