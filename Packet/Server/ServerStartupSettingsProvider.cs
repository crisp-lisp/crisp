namespace Packet.Server
{
    /// <summary>
    /// An implementation of a service that provides a server with a set of initializaton parameters.
    /// </summary>
    internal class ServerStartupSettingsProvider : IServerStartupSettingsProvider
    {
        private readonly ServerStartupSettings _serverStartupSettings;

        /// <summary>
        /// Initializes a new instance of a service that provides a server with a set of initializaton parameters.
        /// </summary>
        /// <param name="serverStartupSettings">The settings this provider should pass back.</param>
        public ServerStartupSettingsProvider(ServerStartupSettings serverStartupSettings)
        {
            _serverStartupSettings = serverStartupSettings;
        }

        public ServerStartupSettings GetSettings()
        {
            return _serverStartupSettings;
        }
    }
}
