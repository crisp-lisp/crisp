using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crisp.Core.Preprocessing
{
    public interface IRequirePathExtractor
    {
        string Extract(string sequence);
    }
}
