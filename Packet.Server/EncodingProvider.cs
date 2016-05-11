using System.Text;

using Crisp.Shared;

using Packet.Interfaces.Server;

namespace Packet.Server
{
    public class EncodingProvider : Provider<Encoding>, IEncodingProvider
    {
        public EncodingProvider(Encoding encoding) : base(encoding)
        {
            
        }
    }
}
