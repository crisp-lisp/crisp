using System.Collections;

namespace Crisp.Core.Tokenizing
{
    /// <summary>
    /// Provides a static factory for different types of tokenizer.
    /// </summary>
    public static class TokenizerFactory
    {
        /// <summary>
        /// Gets a tokenizer configured to tokenize Crisp source files.
        /// </summary>
        /// <returns></returns>
        public static ITokenizer GetCrispTokenizer()
        {
            var tokenizer = new Tokenizer
            {
                IgnoreWhitespace = false
            };
            tokenizer.Add("^#import\\s+\".+?\"\r?$", TokenType.PreprocessorImportStatement);
            tokenizer.Add(";;.+?\r?$", TokenType.PreprocessorComment);
            tokenizer.Add(@"[\(]", TokenType.OpeningParenthesis);
            tokenizer.Add(@"[\)]", TokenType.ClosingParenthesis);
            tokenizer.Add("\"[^\"]*\"", TokenType.String);
            tokenizer.Add(@"[-+]?[0-9]\d*(\.\d+)?", TokenType.Numeric);
            tokenizer.Add(@"\.", TokenType.Dot);
            tokenizer.Add(@"[^\s\(\)\.]+", TokenType.Symbol);
            tokenizer.Add("\\s+", TokenType.PreprocessorWhitespace);

            return tokenizer;
        }
    }
}
