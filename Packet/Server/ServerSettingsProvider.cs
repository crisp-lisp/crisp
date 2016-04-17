using Crisp.Common;

namespace Packet.Server
{
    /// <summary>
    /// An implementation of a service that provides a server with a set of initialization parameters.
    /// </summary>
    internal class ServerSettingsProvider : Provider<ServerSettings>, IServerSettingsProvider
    {
        /// <summary>
        /// Initializes a new instance of a service that provides a server with a set of initialization parameters.
        /// </summary>
        /// <param name="serverSettings">The settings this provider should pass back.</param>
        public ServerSettingsProvider(ServerSettings serverSettings) : base(serverSettings)
        {
        }
    }
}
