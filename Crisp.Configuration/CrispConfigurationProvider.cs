using System.IO;

using Newtonsoft.Json;

using Crisp.Shared;

namespace Crisp.Configuration
{
    /// <summary>
    /// An implementation of an application configuration settings provider.
    /// </summary>
    public class CrispConfigurationProvider : ICrispConfigurationProvider
    {
        private readonly IInterpreterDirectoryPathProvider _interpreterDirectoryPathProvider;

        private ICrispConfiguration _loadedConfiguration;

        public ICrispConfiguration GetConfiguration()
        {
            // Load configuration if not already loaded.
            if (_loadedConfiguration == null)
            {
                var path = Path.Combine(_interpreterDirectoryPathProvider.Get(), "crisp.json");
                var text = File.ReadAllText(path);
                _loadedConfiguration = JsonConvert.DeserializeObject<CrispConfiguration>(text);
            }

            return _loadedConfiguration;
        }

        /// <summary>
        /// Initializes a new instance of an application configuration settings provider.
        /// </summary>
        /// <param name="interpreterDirectoryPathProvider">A service capable of providing the directory path of the executing application.</param>
        public CrispConfigurationProvider(IInterpreterDirectoryPathProvider interpreterDirectoryPathProvider)
        {
            _interpreterDirectoryPathProvider = interpreterDirectoryPathProvider;
        }
    }
}
