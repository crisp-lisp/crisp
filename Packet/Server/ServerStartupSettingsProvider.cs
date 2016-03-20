using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Packet.Server
{
    internal class ServerStartupSettingsProvider : IServerStartupSettingsProvider
    {
        private readonly ServerStartupSettings _serverStartupSettings;

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
