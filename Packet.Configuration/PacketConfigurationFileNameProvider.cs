using Crisp.Shared;
using Packet.Interfaces.Configuration;

namespace Packet.Configuration
{
    public class PacketConfigurationFileNameProvider : Provider<string>, IPacketConfigurationFileNameProvider
    {
        /// <summary>
        /// Initializes a new instance of a Packet configuration file name provider service.
        /// </summary>
        /// <param name="value">The name of the configuration file on-disk.</param>
        public PacketConfigurationFileNameProvider(string value) : base(value)
        {
        }
    }
}
