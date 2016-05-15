using System;
using System.Net.Sockets;

namespace Packet.Interfaces.Server
{
    /// <summary>
    /// Represents a HTTP connection handler.
    /// </summary>
    public interface IHttpConnectionHandler
    {
        /// <summary>
        /// Handles the HTTP connection on the provided <see cref="TcpClient"/> instance.
        /// </summary>
        /// <param name="client">The client on which to handle the connection.</param>
        void Handle(TcpClient client);
    }
}
