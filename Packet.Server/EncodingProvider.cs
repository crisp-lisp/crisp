using System.Text;

using Crisp.Shared;

using Packet.Interfaces.Server;

namespace Packet.Server
{
    public class EncodingProvider : Provider<Encoding>, IEncodingProvider
    {
        /// <summary>
        /// Initializes a new instance of a text encoding provider service.
        /// </summary>
        /// <param name="encoding">The text encoding to be passed back by this service.</param>
        public EncodingProvider(Encoding encoding) : base(encoding)
        {   
        }
    }
}
