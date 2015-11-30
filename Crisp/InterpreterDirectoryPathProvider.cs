using System.IO;
using System.Reflection;

using Crisp.Core.Preprocessing;

namespace Crisp
{
    internal class InterpreterDirectoryPathProvider : IInterpreterDirectoryPathProvider
    {
        public string GetPath()
        {
            return new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName;
        }
    }
}
