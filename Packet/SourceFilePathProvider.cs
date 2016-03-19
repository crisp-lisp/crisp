using Crisp.Core.Preprocessing;

namespace Packet
{
    /// <summary>
    /// An implementation of a service that returns the file path of the file to be interpreted.
    /// </summary>
    internal class SourceFilePathProvider : ISourceFilePathProvider
    {
        private readonly string _path;
        
        public string GetPath()
        {
            return _path;
        }

        /// <summary>
        /// Initializes a new instance of a service that returns the file path of the file to be interpreted.
        /// </summary>
        /// <param name="path">The file path this service should return.</param>
        public SourceFilePathProvider(string path)
        {
            _path = path;
        }
    }
}
