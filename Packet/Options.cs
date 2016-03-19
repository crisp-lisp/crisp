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
        [Option('p', "port", Required = false, HelpText = "Input file to be interpreted.",
            DefaultValue = 8080)]
        public int Port { get; set; }
    }
}
