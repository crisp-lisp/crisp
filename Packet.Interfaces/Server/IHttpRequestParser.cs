namespace Packet.Interfaces.Server
{
    /// <summary>
    /// Represents a HTTP request parser.
    /// </summary>
    public interface IHttpRequestParser
    {
        /// <summary>
        /// Gets or sets the fallback parser to use in case of failure.
        /// </summary>
        IHttpRequestParser Successor { get; set; }

        /// <summary>
        /// Parses the raw HTTP request given.
        /// </summary>
        /// <param name="request">The raw bytes of the request.</param>
        /// <returns></returns>
        IHttpRequest Parse(byte[] request);

        /// <summary>
        /// Gets the version from the HTTP request line given.
        /// </summary>
        /// <param name="requestLine">The request line of the request.</param>
        /// <returns></returns>
        IHttpVersion GetVersion(string requestLine);
    }
}
