using System.IO;

using Packet.Interfaces.Server;

namespace Packet.Server
{
    public abstract class HttpResponse : IHttpResponse
    {
        public IHttpVersion Version { get; }

        protected HttpResponse(IHttpVersion version)
        {
            Version = version;
        }

        public abstract void WriteTo(Stream stream);
    }
}
