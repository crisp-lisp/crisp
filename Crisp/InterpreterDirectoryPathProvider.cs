using System.IO;
using System.Reflection;

using Crisp.Core.Preprocessing;

namespace Crisp
{
    public class InterpreterDirectoryPathProvider : IDirectoryPathProvider
    {
        public string GetPath()
        {
            return new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName;
        }
    }
}
