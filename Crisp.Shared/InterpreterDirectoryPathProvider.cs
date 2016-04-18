using System.IO;
using System.Reflection;

namespace Crisp.Shared
{
    /// <summary>
    /// An implementation of an interpreter directory path provider, capable of returning the directory that contains  
    /// the currently executing interpreter.
    /// </summary>
    public class InterpreterDirectoryPathProvider : IInterpreterDirectoryPathProvider
    {
        public string Get()
        {
            return new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName;
        }
    }
}
