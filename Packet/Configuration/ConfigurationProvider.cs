using System.IO;
using Crisp.Interfaces.Shared;
using Newtonsoft.Json;

using Crisp.Shared;

namespace Packet.Configuration
{
    /// <summary>
    /// An implementation of an application configuration settings provider.
    /// </summary>
    internal class ConfigurationProvider : Provider<Configuration>, IConfigurationProvider
    {
        /// <summary>
        /// Initializes a new instance of an application configuration settings provider.
        /// </summary>
        /// <param name="interpreterDirectoryPathProvider">A service capable of providing the directory path of the executing application.</param>
        public ConfigurationProvider(IInterpreterDirectoryPathProvider interpreterDirectoryPathProvider)
        {
            var path = Path.Combine(interpreterDirectoryPathProvider.Get(), "packet.json");
            var text = File.ReadAllText(path);
            Value = JsonConvert.DeserializeObject<Configuration>(text);
        }
    }
}
