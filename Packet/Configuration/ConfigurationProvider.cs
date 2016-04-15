using System.IO;

using Crisp.Common;
using Crisp.Core.Preprocessing;

using Newtonsoft.Json;

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
            Obj = JsonConvert.DeserializeObject<Configuration>(text);
        }
    }
}
