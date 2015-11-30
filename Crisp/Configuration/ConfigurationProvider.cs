using System.IO;

using Newtonsoft.Json;

using Crisp.Core.Preprocessing;

namespace Crisp.Configuration
{
    /// <summary>
    /// Represents an application configuration settings provider.
    /// </summary>
    internal class ConfigurationProvider : IConfigurationProvider
    {
        private readonly IInterpreterDirectoryPathProvider _interpreterDirectoryPathProvider;

        private Configuration _loadedConfiguration;

        public Configuration GetConfiguration()
        {
            if (_loadedConfiguration == null)
            {
                _loadedConfiguration = JsonConvert.DeserializeObject<Configuration>(File.ReadAllText(Path.Combine(_interpreterDirectoryPathProvider.GetPath(), "config.json")));
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
