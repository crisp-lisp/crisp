using System.Reflection;

using Crisp.Interfaces.Shared;

namespace Crisp.Shared
{
    public class InterpreterFilePathProvider : Provider<string>, IInterpreterFilePathProvider
    {
        /// <summary>
        /// Initializes a new instance of a file path provider that returns the path of the interpreter executable.
        /// </summary>
        public InterpreterFilePathProvider() : base(Assembly.GetExecutingAssembly().Location)
        {
        }
    }
}
