using System.Collections.Generic;

using Crisp.Enums;
using Crisp.Interfaces.Tokenization;
using Crisp.Shared;

namespace Crisp.Tokenization
{
    /// <summary>
    /// An implementation of a service for retrieving tokenizer configurations.
    /// </summary>
    public class TokenizerConfigurationProvider 
        : Provider<Dictionary<string, TokenType>>, ITokenizerConfigurationProvider
    {
        /// <summary>
        /// Initializes a new instance of a service for retrieving tokenizer configurations.
        /// </summary>
        /// <param name="value">The </param>
        public TokenizerConfigurationProvider(Dictionary<string, TokenType> value) : base(value)
        {
        }
    }
}
