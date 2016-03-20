using System.IO;

namespace Packet.Server
{
    internal interface IHttpServer
    {
        /// <summary>
        /// Handles a request with a GET HTTP verb submitted to the server.
        /// </summary>
        /// <param name="processor">The processor to use to handle the request.</param>
        void HandleGetRequest(HttpProcessor processor);

        /// <summary>
        /// Handles a request with a POST HTTP verb submitted to the server.
        /// </summary>
        /// <param name="processor">The processor to use to handle the request.</param>
        /// <param name="inputStream">The <see cref="StreamReader"/> to read the request body.</param>
        void HandlePostRequest(HttpProcessor processor, StreamReader inputStream);

        /// <summary>
        /// Begins listening for requests.
        /// </summary>
        void Listen();
    }
}
