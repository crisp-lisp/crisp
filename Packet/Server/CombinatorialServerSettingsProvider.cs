using Packet.Configuration;

namespace Packet.Server
{
    class CombinatorialServerSettingsProvider : IServerSettingsProvider
    {
        private readonly ServerSettings _serverSettings;
        
        public CombinatorialServerSettingsProvider(
            IOptionsProvider optionsProvider, 
            IConfigurationProvider configurationProvider)
        {
            _serverSettings = new ServerSettings
            {
                WebRoot = optionsProvider.Get().WebRoot,
                Port = optionsProvider.Get().Port,
            };
        }

        public ServerSettings GetSettings()
        {
            return _serverSettings;
        }
    }
}
