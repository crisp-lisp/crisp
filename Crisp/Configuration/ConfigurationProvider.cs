using System.IO;

using Newtonsoft.Json;

using Crisp.Core.Preprocessing;

namespace Crisp.Configuration
{
    /// <summary>
    /// An implementation of an application configuration settings provider.
    /// </summary>
    internal class ConfigurationProvider : IConfigurationProvider
    {
        private readonly IInterpreterDirectoryPathProvider _interpreterDirectoryPathProvider;

        private Configuration _loadedConfiguration;

        public Configuration GetConfiguration()
        {
            // Load configuration if not already loaded.
            if (_loadedConfiguration == null)
            {
                var path = Path.Combine(_interpreterDirectoryPathProvider.GetPath(), "config.json");
                var text = File.ReadAllText(path);
                _loadedConfiguration = JsonConvert.DeserializeObject<Configuration>(text);
            }

            return _loadedConfiguration;
        }

        /// <summary>
        /// Initializes a new instance of an application configuration settings provider.
        /// </summary>
        /// <param name="interpreterDirectoryPathProvider">A service capable of providing the directory path of the executing application.</param>
        public ConfigurationProvider(IInterpreterDirectoryPathProvider interpreterDirectoryPathProvider)
        {
            _interpreterDirectoryPathProvider = interpreterDirectoryPathProvider;
        }
    }
}
