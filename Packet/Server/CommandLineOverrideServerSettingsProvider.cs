using Crisp.Common;

using Packet.Configuration;

namespace Packet.Server
{
    internal class CommandLineOverrideServerSettingsProvider : Provider<ServerSettings>, IServerSettingsProvider
    {
        public CommandLineOverrideServerSettingsProvider(
            IOptionsProvider optionsProvider, 
            IConfigurationProvider configurationProvider)
        {
            Value = new ServerSettings
            {
                WebRoot = optionsProvider.Get().WebRoot,
                Port = optionsProvider.Get().Port,
            };
        }
    }
}
