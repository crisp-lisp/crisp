using System.IO;

using Crisp.Interfaces.Shared;

namespace Crisp.Shared
{
    /// <summary>
    /// Represents a file-based source code provider.
    /// </summary>
    public class FileSourceCodeProvider : SourceCodeProvider
    {
        /// <summary>
        /// Initializes a new instance of a file-based source code provider service.
        /// </summary>
        /// <param name="sourceFilePathProvider">The source file path provider service.</param>
        public FileSourceCodeProvider(ISourceFilePathProvider sourceFilePathProvider)
            : base(File.ReadAllText(sourceFilePathProvider.Get()))
        {
        }
    }
}
