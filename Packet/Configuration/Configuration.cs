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

        public IDictionary<string, string> MimeTypeMappings { get; set; }

        public string InternalServerErrorPage { get; set; }

        public string NotFoundErrorPage { get; set; }

        public IEnumerable<string> DirectoryIndices { get; set; } 
    }
}
