using Crisp.Shared;

namespace Crisp.Tokenization
{
    /// <summary>
    /// Provides a static factory for different types of token filter.
    /// </summary>
    public static class TokenFilterFactory
    {
        /// <summary>
        /// Returns a token filter that will remove all comments and whitespace.
        /// </summary>
        /// <returns></returns>
        public static ITokenFilter GetCommentAndWhitespaceFilter()
        {
            return new TokenFilter(new[]
            {
                TokenType.Whitespace, 
                TokenType.Comment, 
            });
        }
    }
}
