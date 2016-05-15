using Newtonsoft.Json;

using Crisp.Shared;
using Packet.Interfaces.Configuration;

namespace Packet.Configuration
{
    public class PacketConfigurationProvider : Provider<IPacketConfiguration>, IPacketConfigurationProvider
    {
        /// <summary>
        /// Initializes a new instance of an application configuration settings provider service.
        /// </summary>
        /// <param name="rawPacketConfigurationProvider">The raw configuration provider service.</param>
        public PacketConfigurationProvider(IRawPacketConfigurationProvider rawPacketConfigurationProvider)
        {
            var rawConfigurationFileText = rawPacketConfigurationProvider.Get();
            Value = JsonConvert.DeserializeObject<PacketConfiguration>(rawConfigurationFileText); // Use JSON.
        }
    }
}
