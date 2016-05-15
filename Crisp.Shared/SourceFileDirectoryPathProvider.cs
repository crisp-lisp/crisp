using System.IO;

using Crisp.Interfaces.Shared;

namespace Crisp.Shared
{
    public class SourceFileDirectoryPathProvider : Provider<string>, ISourceFileDirectoryPathProvider
    {
        /// <summary>
        /// Initializes a new instance of a service that returns the directory path of the source file.
        /// </summary>
        /// <param name="sourceFilePathProvider">The source file path provider service.</param>
        public SourceFileDirectoryPathProvider(ISourceFilePathProvider sourceFilePathProvider) 
            : base(Path.GetDirectoryName(sourceFilePathProvider.Get()))
        {
        }
    }
}
