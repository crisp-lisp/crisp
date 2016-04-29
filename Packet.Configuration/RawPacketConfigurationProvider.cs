using System.IO;

using Crisp.Interfaces.Shared;
using Crisp.Shared;
using Packet.Interfaces.Configuration;

namespace Packet.Configuration
{
    public class RawPacketConfigurationProvider : Provider<string>, IRawPacketConfigurationProvider
    {
        /// <summary>
        /// Initializes a new instance of a service to load the raw text of a Packet configuration file.
        /// </summary>
        /// <param name="interpreterDirectoryPathProvider">The interpreter directory path provider service.</param>
        /// <param name="packetConfigurationFileNameProvider">The configuration file name provider service.</param>
        public RawPacketConfigurationProvider(
            IInterpreterDirectoryPathProvider interpreterDirectoryPathProvider,
            IPacketConfigurationFileNameProvider packetConfigurationFileNameProvider)
        {
            // Calculate path and check file exists.
            var path = Path.Combine(interpreterDirectoryPathProvider.Get(),
                packetConfigurationFileNameProvider.Get());
            if (!File.Exists(path))
            {
                throw new FileNotFoundException($"Packet configuration file at '{path}' not found.");
            }

            // Return file text.
            Value = File.ReadAllText(path);
        }
    }
}
