using System.Collections.Generic;

using Crisp.Shared;

namespace Crisp.Tokenization
{
    /// <summary>
    /// A static factory class for retrieving <see cref="ITokenizerConfigurationProvider"/> instances.
    /// </summary>
    public static class TokenizerConfigurationProviderFactory
    {
        /// <summary>
        /// Gets the tokenizer configuration provider for Crisp code.
        /// </summary>
        /// <returns></returns>
        public static ITokenizerConfigurationProvider GetCrispTokenizerConfigurationProvider()
        {
            return new TokenizerConfigurationProvider(new Dictionary<string, TokenType>
            {
                {"\"[^\"]*\"", TokenType.String},
                {";;.+?\r?$", TokenType.Comment},
                {@"[\(]", TokenType.OpeningParenthesis},
                {@"[\)]", TokenType.ClosingParenthesis},
                {@"[-+]?[0-9]\d*(\.\d+)?", TokenType.Numeric},
                {@"\.", TokenType.Dot},
                {@"true", TokenType.BooleanTrue},
                {@"false", TokenType.BooleanFalse},
                {@"nil", TokenType.Nil},
                {@"[^\s\(\)\.]+", TokenType.Symbol},
                {"\\s+", TokenType.Whitespace}
            });
        } 
    }
}
