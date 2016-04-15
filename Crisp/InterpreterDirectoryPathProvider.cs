using System.IO;
using System.Reflection;

using Crisp.Core.Preprocessing;

namespace Crisp
{
    /// <summary>
    /// An implementation of an interpreter directory path provider, capable of returning the directory that contains  
    /// the currently executing interpreter.
    /// </summary>
    internal class InterpreterDirectoryPathProvider : IInterpreterDirectoryPathProvider
    {
        public string Get()
        {
            return new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName;
        }
    }
}
