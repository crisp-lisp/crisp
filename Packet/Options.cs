using CommandLine;

namespace Packet
{
    /// <summary>
    /// Represents a container object to hold arguments passed via the command line.
    /// </summary>
    internal class Options
    {
        /// <summary>
        /// Gets or sets the port that the server should be set listening on.
        /// </summary>
        [Option('p', "port", Required = false, HelpText = "The port to start the server on.",
            DefaultValue = 8080)]
        public int Port { get; set; }

        /// <summary>
        /// Gets or sets the directory that acts as the public web root for the server.
        /// </summary>
        [Option('r', "root", Required = false, HelpText = "The directory to serve.", DefaultValue = "public-www")]
        public string WebRoot { get; set; }
    }
}
