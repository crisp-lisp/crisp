using System.Collections.Generic;

using Crisp.Enums;
using Crisp.Interfaces.Tokenization;

namespace Crisp.Testing.Common
{
    /// <summary>
    /// A helper class for producing lists of mock <see cref="IToken"/> instances.
    /// </summary>
    public static class MockTokenListFactory
    {
        /// <summary>
        /// Gets a token list containing the symbolic tokens a-e.
        /// </summary>
        /// <returns></returns>
        public static IList<IToken> GetSingleTokenList()
        {
            return new List<IToken>
            {
                MockTokenFactory.GetMockToken(TokenType.Symbol, "a"),
                MockTokenFactory.GetMockToken(TokenType.Symbol, "b"),
                MockTokenFactory.GetMockToken(TokenType.Symbol, "c"),
                MockTokenFactory.GetMockToken(TokenType.Symbol, "d"),
                MockTokenFactory.GetMockToken(TokenType.Symbol, "e")
            };
        }

        /// <summary>
        /// Gets a token list containing a numeric followed by two proper lists containing two numerics each.
        /// </summary>
        /// <returns></returns>
        public static IList<IToken> GetMixedAtomicityTokenList()
        {
            return new List<IToken>
            {
                MockTokenFactory.GetMockToken(TokenType.Numeric, "1"),
                MockTokenFactory.GetMockToken(TokenType.OpeningParenthesis, "("),
                MockTokenFactory.GetMockToken(TokenType.Numeric, "2"),
                MockTokenFactory.GetMockToken(TokenType.Numeric, "3"),
                MockTokenFactory.GetMockToken(TokenType.ClosingParenthesis, ")"),
                MockTokenFactory.GetMockToken(TokenType.OpeningParenthesis, "("),
                MockTokenFactory.GetMockToken(TokenType.Numeric, "4"),
                MockTokenFactory.GetMockToken(TokenType.Numeric, "5"),
                MockTokenFactory.GetMockToken(TokenType.ClosingParenthesis, ")")
            };
        }

        /// <summary>
        /// Gets a token list containing only list expressions, with nesting.
        /// </summary>
        /// <returns></returns>
        public static IList<IToken> GetListExpressionTokenList()
        {
            return new List<IToken>
            {
                MockTokenFactory.GetMockToken(TokenType.OpeningParenthesis, "("),
                MockTokenFactory.GetMockToken(TokenType.Numeric, "1"),
                MockTokenFactory.GetMockToken(TokenType.OpeningParenthesis, "("),
                MockTokenFactory.GetMockToken(TokenType.Numeric, "2"),
                MockTokenFactory.GetMockToken(TokenType.Numeric, "3"),
                MockTokenFactory.GetMockToken(TokenType.ClosingParenthesis, ")"),
                MockTokenFactory.GetMockToken(TokenType.ClosingParenthesis, ")"),
                MockTokenFactory.GetMockToken(TokenType.OpeningParenthesis, "("),
                MockTokenFactory.GetMockToken(TokenType.Numeric, "4"),
                MockTokenFactory.GetMockToken(TokenType.Numeric, "5"),
                MockTokenFactory.GetMockToken(TokenType.ClosingParenthesis, ")")
            };
        }

        /// <summary>
        /// Gets a token list containing one list expression, with nesting.
        /// </summary>
        /// <returns></returns>
        public static IList<IToken> GetEntireListExpressionTokenList()
        {
            return new List<IToken>
            {
                MockTokenFactory.GetMockToken(TokenType.OpeningParenthesis, "("),
                MockTokenFactory.GetMockToken(TokenType.Numeric, "1"),
                MockTokenFactory.GetMockToken(TokenType.OpeningParenthesis, "("),
                MockTokenFactory.GetMockToken(TokenType.Numeric, "2"),
                MockTokenFactory.GetMockToken(TokenType.Numeric, "3"),
                MockTokenFactory.GetMockToken(TokenType.ClosingParenthesis, ")"),
                MockTokenFactory.GetMockToken(TokenType.Numeric, "4"),
                MockTokenFactory.GetMockToken(TokenType.Numeric, "5"),
                MockTokenFactory.GetMockToken(TokenType.ClosingParenthesis, ")")
            };
        }

        /// <summary>
        /// Gets a token list containing one list expression, with nesting, comments and whitespace.
        /// </summary>
        /// <returns></returns>
        public static IList<IToken> GetCommentedEntireListExpressionTokenList()
        {
            return new List<IToken>
            {
                MockTokenFactory.GetMockToken(TokenType.Comment, ";; Testing comment."),
                MockTokenFactory.GetMockToken(TokenType.Whitespace, "\r\n"),
                MockTokenFactory.GetMockToken(TokenType.OpeningParenthesis, "("),
                MockTokenFactory.GetMockToken(TokenType.Numeric, "1"),
                MockTokenFactory.GetMockToken(TokenType.OpeningParenthesis, "("),
                MockTokenFactory.GetMockToken(TokenType.Numeric, "2"),
                MockTokenFactory.GetMockToken(TokenType.Numeric, "3"),
                MockTokenFactory.GetMockToken(TokenType.ClosingParenthesis, ")"),
                MockTokenFactory.GetMockToken(TokenType.Numeric, "4"),
                MockTokenFactory.GetMockToken(TokenType.Numeric, "5"),
                MockTokenFactory.GetMockToken(TokenType.ClosingParenthesis, ")"),
                MockTokenFactory.GetMockToken(TokenType.Whitespace, "\r\n")
            };
        } 
    }
}
