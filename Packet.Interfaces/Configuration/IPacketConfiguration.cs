using System.Collections.Generic;

namespace Packet.Interfaces.Configuration
{
    /// <summary>
    /// Represents a set of application configuration settings.
    /// </summary>
    public interface IPacketConfiguration
    {
        /// <summary>
        /// Gets or sets the collection of file extensions that will be interpreted as Crisp files.
        /// </summary>
        IEnumerable<string> CrispFileExtensions { get; set; }

        /// <summary>
        /// Gets or sets a dictionary mapping file extensions to the MIME types they should be served as.
        /// </summary>
        IDictionary<string, string> MimeTypeMappings { get; set; }

        /// <summary>
        /// Gets or sets the path to the configured 500 internal server error page.
        /// </summary>
        string InternalServerErrorPage { get; set; }

        /// <summary>
        /// Gets or sets the path to the configured 404 not found error page.
        /// </summary>
        string NotFoundErrorPage { get; set; }

        /// <summary>
        /// Gets or sets the path to the configured 403 forbidden error page.
        /// </summary>
        string ForbiddenErrorPage { get; set; }

        /// <summary>
        /// Gets or sets the collection of filenames that represent directory index files.
        /// </summary>
        IEnumerable<string> DirectoryIndices { get; set; }

        /// <summary>
        /// Gets or sets a collection of regular expressions that match filepaths that should not be served to clients.
        /// </summary>
        IEnumerable<string> DoNotServePatterns { get; set; }

        /// <summary>
        /// Gets or sets the IP address that this server is bound to.
        /// </summary>
        string BindingIpAddress { get; set; }

        /// <summary>
        /// Gets or sets the port number that this server should listen on.
        /// </summary>
        int Port { get; set; }

        /// <summary>
        /// Gets or sets the maximum allowed post size, in bytes.
        /// </summary>
        int MaxPostSize { get; set; }

        /// <summary>
        /// Gets or sets the path to the web root directory.
        /// </summary>
        string WebRoot { get; set; }
    }
}
