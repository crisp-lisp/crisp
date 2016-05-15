namespace Packet.Interfaces.Server
{
    /// <summary>
    /// Represents a service for retrieving the content of error pages.
    /// </summary>
    public interface IErrorPageContentRetriever
    {
        /// <summary>
        /// Gets the content of the error page corresponding to the given status code.
        /// </summary>
        /// <param name="errorStatusCode">The status code corresponding to the error page to get.</param>
        /// <returns></returns>
        string GetErrorPageContent(int errorStatusCode);

        /// <summary>
        /// Gets the encoded content of the error page corresponding to the given status code.
        /// </summary>
        /// <param name="errorStatusCode">The status code corresponding to the error page to get.</param>
        /// <returns></returns>
        byte[] GetEncodedErrorPageContent(int errorStatusCode);
    }
}
