using Packet.Interfaces.Server;

namespace Packet.Server
{
    public class HttpVersion : IHttpVersion
    {
        public int Major { get; set; }

        public int Minor { get; set; }

        /// <summary>
        /// Initializes a new instance of a response to a HTTP request.
        /// </summary>
        /// <param name="major">The major version number.</param>
        /// <param name="minor">The minor version number.</param>
        public HttpVersion(int major, int minor)
        {
            Major = major;
            Minor = minor;
        }
        public bool IsVersion(int major, int minor)
        {
            return Major == major && Minor == minor;
        }

        public override string ToString()
        {
            return $"HTTP/{Major}.{Minor}"; // Formatted as a standard HTTP version string.
        }
    }
}
