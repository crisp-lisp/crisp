using System.IO;

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
