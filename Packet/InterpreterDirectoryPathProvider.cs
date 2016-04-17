using System.IO;
using System.Reflection;

using Crisp.Shared;

namespace Packet
{
    /// <summary>
    /// An implementation of a service that returns the path of the directory that contains the currently executing 
    /// interpreter.
    /// </summary>
    internal class InterpreterDirectoryPathProvider : Provider<string>, IInterpreterDirectoryPathProvider
    {
        /// <summary>
        /// An implementation of a service that returns the path of the directory that contains the currently executing 
        /// interpreter.
        /// </summary>
        public InterpreterDirectoryPathProvider()
        {
            Value = new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName;
        }
    }
}
