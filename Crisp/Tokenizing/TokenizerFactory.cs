namespace Crisp.Tokenizing
{
    /// <summary>
    /// Provides a static factory for different types of tokenizer.
    /// </summary>
    internal static class TokenizerFactory
    {
        /// <summary>
        /// Gets a tokenizer configured to tokenize Crisp source files.
        /// </summary>
        /// <returns></returns>
        public static Tokenizer GetCrispTokenizer()
        {
            var tokenizer = new Tokenizer();
            tokenizer.Add(@"[\(]", TokenType.OpeningParenthesis);
            tokenizer.Add(@"[\)]", TokenType.ClosingParenthesis);
            tokenizer.Add("\"[^\"]*\"", TokenType.String);
            tokenizer.Add(@"[-+]?[0-9]\d*(\.\d+)?", TokenType.Numeric);
            tokenizer.Add(@"\.", TokenType.Dot);
            tokenizer.Add(@"[^\s\(\)\.]+", TokenType.Symbol);

            return tokenizer;
        }
    }
}
