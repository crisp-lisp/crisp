using Crisp.Interfaces.Shared;

namespace Crisp.Shared
{
    public class SourceCodeProvider : Provider<string>, ISourceCodeProvider
    {
        /// <summary>
        /// Initializes a new instance of a source code provider service.
        /// </summary>
        /// <param name="sourceCode">The source code this service should provide.</param>
        public SourceCodeProvider(string sourceCode) : base(sourceCode)
        {
        }
    }
}
