using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crisp.Core.Preprocessing;

namespace Crisp
{
    internal class SourceFilePathProvider : ISourceFilePathProvider
    {
        private readonly string _path;
        
        public string GetPath()
        {
            return _path;
        }

        public SourceFilePathProvider(string path)
        {
            _path = path;
        }
    }
}
