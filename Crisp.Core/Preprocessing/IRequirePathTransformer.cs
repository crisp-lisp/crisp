using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crisp.Core.Preprocessing
{
    public interface IRequirePathTransformer
    {
        string Transform(string path);
    }
}
