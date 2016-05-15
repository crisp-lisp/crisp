using System.Collections.Generic;

using Crisp.Enums;

namespace Crisp.Interfaces.Tokenization
{
    /// <summary>
    /// Represents a tokenizer configuration provider service.
    /// </summary>
    public interface ITokenizerConfigurationProvider
    {
        /// <summary>
        /// Gets a dictionary mapping regular expression strings to their corresponding token types.
        /// </summary>
        /// <returns></returns>
        Dictionary<string, TokenType> Get();
    }
}
