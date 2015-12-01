using System.Text.RegularExpressions;

namespace Crisp.Core.Preprocessing
{
    /// <summary>
    /// An implementation of a require path extractor, capable of extracting the raw filepath of a required file from a 
    /// string representing the require statement token sequence.
    /// </summary>
    public class RequirePathExtractor : IRequirePathExtractor
    {
        public string Extract(string sequence)
        {
            // Pull contents out of quotes.
            return Regex.Match(sequence, "\"(.+?)\"").Captures[0].Value.Trim('\"');
        }
    }
}
