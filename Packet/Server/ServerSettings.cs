namespace Packet.Server
{
    /// <summary>
    /// Represents a set of information passed to the server to initialize it.
    /// </summary>
    internal class ServerSettings
    {
        /// <summary>
        /// The port to listen on.
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// The directory path of the folder that serves as the web root directory.
        /// </summary>
        public string WebRoot { get; set; }
    }
}
