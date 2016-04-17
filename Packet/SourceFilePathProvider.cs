using Crisp.Shared;

namespace Packet
{
    /// <summary>
    /// An implementation of a service that returns the path of the source file.
    /// </summary>
    internal class SourceFilePathProvider : Provider<string>, ISourceFilePathProvider
    {
        /// <summary>
        /// Initializes a new instance of a service that returns the path of the source file.
        /// </summary>
        /// <param name="path">The path of the source file.</param>
        public SourceFilePathProvider(string path) : base(path)
        {
        }
    }
}
