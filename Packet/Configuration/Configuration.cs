using System.Collections.Generic;

namespace Packet.Configuration
{
    /// <summary>
    /// Represents a set of application configuration settings.
    /// </summary>
    internal class Configuration
    {
        /// <summary>
        /// Gets or sets the directory that special form libraries will be loaded from.
        /// </summary>
        public string SpecialFormDirectory { get; set; }

        /// <summary>
        /// Gets or sets the collection of file extensions that will be interpreted as Crisp files.
        /// </summary>
        public IEnumerable<string> CrispFileExtensions { get; set; }

        /// <summary>
        /// Gets or sets a dictionary mapping file extensions to the MIME types they should be served as.
        /// </summary>
        public IDictionary<string, string> MimeTypeMappings { get; set; }

        /// <summary>
        /// Gets or sets the path to the configured 500 internal server error page.
        /// </summary>
        public string InternalServerErrorPage { get; set; }

        /// <summary>
        /// Gets or sets the path to the configured 404 not found error page.
        /// </summary>
        public string NotFoundErrorPage { get; set; }

        /// <summary>
        /// Gets or sets the collection of filenames that represent directory index files.
        /// </summary>
        public IEnumerable<string> DirectoryIndices { get; set; } 

        /// <summary>
        /// Gets or sets a collection of regular expressions that match filepaths that should not be served to clients.
        /// </summary>
        public IEnumerable<string> DoNotServePatterns { get; set; }
    }
}
