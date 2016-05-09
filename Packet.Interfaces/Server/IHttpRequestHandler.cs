namespace Packet.Interfaces.Server
{
    /// <summary>
    /// Represents a HTTP request handler.
    /// </summary>
    public interface IHttpRequestHandler
    {
        /// <summary>
        /// Gets or sets the handler to use in case of failure.
        /// </summary>
        IHttpRequestHandler Successor { get; set; }

        /// <summary>
        /// Computes and returns a response for the given HTTP request.
        /// </summary>
        /// <param name="request">The request to compute a response for.</param>
        /// <returns></returns>
        IHttpResponse Handle(IHttpRequest request);
    }
}
