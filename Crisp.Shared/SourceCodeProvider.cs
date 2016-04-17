using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crisp.Shared
{
    public class SourceCodeProvider : Provider<string>, ISourceCodeProvider
    {
        public SourceCodeProvider(string sourceCode) : base(sourceCode)
        {
        }
    }
}
