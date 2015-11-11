using System.Collections.Generic;

namespace Crisp.Core.Tokenizing
{
    /// <summary>
    /// Implemented by classes usable as tokenization engines.
    /// </summary>
    public interface ITokenizer
    {
        /// <summary>
        /// Tokenizes a string.
        /// </summary>
        /// <param name="source">The string to tokenize.</param>
        /// <returns></returns>
        IList<Token> Tokenize(string source);
    }
}
