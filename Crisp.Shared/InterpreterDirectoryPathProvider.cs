using System.IO;

using Crisp.Interfaces.Shared;

namespace Crisp.Shared
{
    public class InterpreterDirectoryPathProvider : Provider<string>, IInterpreterDirectoryPathProvider
    {
        /// <summary>
        /// Initializes a new instance of a service that returns the directory path of the interpreter executable.
        /// </summary>
        /// <param name="interpreterFilePathProvider">The interpreter file path provider service.</param>
        public InterpreterDirectoryPathProvider(IInterpreterFilePathProvider interpreterFilePathProvider) 
            : base(Path.GetDirectoryName(interpreterFilePathProvider.Get()))
        {
        }
    }
}
