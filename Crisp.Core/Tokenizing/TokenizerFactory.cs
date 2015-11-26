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
        /// <param name="ignoreWhitespace">Whether or not to include whitespace tokens in the output.</param>
        /// <returns></returns>
        public static ITokenizer GetCrispTokenizer(bool ignoreWhitespace = false)
        {
            var tokenizer = new Tokenizer
            {
                IgnoreWhitespace = ignoreWhitespace
            };
            tokenizer.Add("^#import\\s+\".+?\"\r?$", TokenType.RequireStatement);
            tokenizer.Add(";;.+?\r?$", TokenType.Comment);
            tokenizer.Add(@"[\(]", TokenType.OpeningParenthesis);
            tokenizer.Add(@"[\)]", TokenType.ClosingParenthesis);
            tokenizer.Add("\"[^\"]*\"", TokenType.String);
            tokenizer.Add(@"[-+]?[0-9]\d*(\.\d+)?", TokenType.Numeric);
            tokenizer.Add(@"\.", TokenType.Dot);
            tokenizer.Add(@"true", TokenType.BooleanTrue);
            tokenizer.Add(@"false", TokenType.BooleanFalse);
            tokenizer.Add(@"nil", TokenType.Nil);
            tokenizer.Add(@"[^\s\(\)\.]+", TokenType.Symbol);
            tokenizer.Add("\\s+", TokenType.Whitespace);

            return tokenizer;
        }
    }
}
