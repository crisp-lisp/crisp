namespace Crisp.Core.Tokenizing
{
    /// <summary>
    /// Provides a static factory for different types of token filter.
    /// </summary>
    public class TokenFilterFactory
    {
        /// <summary>
        /// Returns a token filter that will remove all comments and whitespace.
        /// </summary>
        /// <returns></returns>
        public static TokenFilter GetCommentAndWhitespaceFilter()
        {
            return new TokenFilter(new[]
            {
                TokenType.Whitespace, 
                TokenType.Comment, 
            });
        }

        /// <summary>
        /// Returns a token filter that will remove all comments, whitespace and directives.
        /// </summary>
        /// <returns></returns>
        public static TokenFilter GetCommentWhitespaceAndDirectiveFilter()
        {
            return new TokenFilter(new[]
            {
                TokenType.Whitespace, 
                TokenType.Comment, 
                TokenType.RequireStatement,
            });
        }
    }
}
