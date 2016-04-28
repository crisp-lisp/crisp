using System.IO;
using Crisp.Interfaces.Shared;

namespace Crisp.Shared
{
    public class FileSourceCodeProvider : SourceCodeProvider
    {
        public FileSourceCodeProvider(ISourceFilePathProvider sourceFilePathProvider)
            : base(File.ReadAllText(sourceFilePathProvider.Get()))
        {
        }
    }
}
