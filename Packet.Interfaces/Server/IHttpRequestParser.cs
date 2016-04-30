namespace Packet.Interfaces.Server
{
    /// <summary>
    /// Represents a HTTP request parser.
    /// </summary>
    public interface IHttpRequestParser
    {
        /// <summary>
        /// Parses the raw HTTP request given, as read from a network socket.
        /// </summary>
        /// <param name="request">The raw bytes of the request.</param>
        /// <returns></returns>
        IHttpRequest Parse(byte[] request);
    }
}
