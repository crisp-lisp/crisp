namespace Packet.Interfaces.Configuration
{
    /// <summary>
    /// Represents a Packet configuration file name provider service.
    /// </summary>
    public interface IPacketConfigurationFileNameProvider
    {
        /// <summary>
        /// Gets the name of the configuration file on-disk.
        /// </summary>
        /// <returns></returns>
        string Get();
    }
}