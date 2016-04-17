using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
