using Packet.Interfaces.Server;

namespace Packet.Server
{
    public class HttpVersion : IHttpVersion
    {
        public int Major { get; set; }

        public int Minor { get; set; }

        public override string ToString()
        {
            return $"HTTP/{Major}.{Minor}";
        }

        public HttpVersion(int major, int minor)
        {
            Major = major;
            Minor = minor;
        }
    }
}
