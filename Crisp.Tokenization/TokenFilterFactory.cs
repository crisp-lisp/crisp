using Crisp.Enums;
using Crisp.Interfaces.Tokenization;

namespace Crisp.Tokenization
{
    /// <summary>
    /// Represents a factory for producing <see cref="ITokenFilter"/> instances.
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
                TokenType.Comment
            });
        }
    }
}
