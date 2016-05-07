namespace Packet.Interfaces.Server
{
    /// <summary>
    /// Represents a HTTP server.
    /// </summary>
    public interface IHttpServer
    {
        /// <summary>
        /// Begins listening for requests.
        /// </summary>
        void Listen();
    }
}
