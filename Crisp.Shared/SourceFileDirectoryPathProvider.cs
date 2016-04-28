using System;
using System.IO;
using Crisp.Interfaces.Shared;

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
