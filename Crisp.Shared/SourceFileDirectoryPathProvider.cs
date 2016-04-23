using System;
using System.IO;

namespace Crisp.Shared
{
    public class SourceFileDirectoryPathProvider : Provider<string>, ISourceFileDirectoryPathProvider
    {
        public SourceFileDirectoryPathProvider(ISourceFilePathProvider sourceFilePathProvider) 
            : base(Path.GetDirectoryName(sourceFilePathProvider.Get()))
        {
        }
    }
}
