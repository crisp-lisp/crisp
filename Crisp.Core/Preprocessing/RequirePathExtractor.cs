using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Crisp.Core.Preprocessing
{
    public class RequirePathExtractor : IRequirePathExtractor
    {
        public string Extract(string sequence)
        {
            return Regex.Match(sequence, "\"(.+?)\"").Captures[0].Value.Trim('\"');
        }
    }
}
