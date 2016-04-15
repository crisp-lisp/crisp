namespace Packet.Server
{
    /// <summary>
    /// Represents a service that provides a server with a set of initializaton parameters.
    /// </summary>
    internal interface IServerSettingsProvider
    {
        /// <summary>
        /// Gets the set of initialization parameters necessary to start the server.
        /// </summary>
        /// <returns></returns>
        ServerSettings GetSettings();
    }
}
