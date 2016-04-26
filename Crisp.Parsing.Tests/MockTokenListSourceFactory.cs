using System.Collections.Generic;

using Moq;

using Crisp.Enums;
using Crisp.Interfaces.Tokenization;

namespace Crisp.Parsing.Tests
{
    /// <summary>
    /// A helper class for producing mock <see cref="ITokenListSource"/> instances.
    /// </summary>
    public static class MockTokenListSourceFactory
    {
        /// <summary>
        /// Returns a mocked token.
        /// </summary>
        /// <param name="tokenType">The type to assign to the mock token.</param>
        /// <param name="sequence">The sequence of text to assign to the mock token.</param>
        /// <param name="column">The column number to assign to the mock token.</param>
        /// <param name="line">The line number to assign to the mock token.</param>
        /// <returns></returns>
        public static IToken GetMockToken(TokenType tokenType, string sequence, int column = -1, int line = -1)
        {
            var token = new Mock<IToken>();
            token.SetupGet(t => t.Type).Returns(tokenType);
            token.SetupGet(t => t.Sequence).Returns(sequence);
            token.SetupGet(t => t.Column).Returns(column);
            token.SetupGet(t => t.Line).Returns(line);

            return token.Object;
        }

        /// <summary>
        /// Gets a valid token list source comprising a proper list containing the numerics 1-3.
        /// </summary>
        /// <returns></returns>
        public static ITokenListSource GetProperListTokenListSource()
        {
            var source = new Mock<ITokenListSource>();
            source.Setup(s => s.Get())
                .Returns(new List<IToken>
                {
                    GetMockToken(TokenType.OpeningParenthesis, "("),
                    GetMockToken(TokenType.Numeric, "1"),
                    GetMockToken(TokenType.Numeric, "2"),
                    GetMockToken(TokenType.Numeric, "3"),
                    GetMockToken(TokenType.ClosingParenthesis, ")")
                });

            return source.Object;
        }

        /// <summary>
        /// Gets a valid token list source comprising an improper list containing the numerics 1-3, terminated by 3.
        /// </summary>
        /// <returns></returns>
        public static ITokenListSource GetImproperListTokenListSource()
        {
            var source = new Mock<ITokenListSource>();
            source.Setup(s => s.Get())
                .Returns(new List<IToken>
                {
                    GetMockToken(TokenType.OpeningParenthesis, "("),
                    GetMockToken(TokenType.Numeric, "1"),
                    GetMockToken(TokenType.Numeric, "2"),
                    GetMockToken(TokenType.Numeric, "3"),
                    GetMockToken(TokenType.Dot, "."),
                    GetMockToken(TokenType.Numeric, "4"),
                    GetMockToken(TokenType.ClosingParenthesis, ")")
                });

            return source.Object;
        }

        /// <summary>
        /// Gets an invalid token list source comprising an opening parenthesis followed by the numerics 1-3.
        /// </summary>
        /// <returns></returns>
        public static ITokenListSource GetMismatchedBracketsTokenListSource()
        {
            var source = new Mock<ITokenListSource>();
            source.Setup(s => s.Get())
                .Returns(new List<IToken>
                {
                    GetMockToken(TokenType.OpeningParenthesis, "(", 1, 1),
                    GetMockToken(TokenType.Numeric, "1"),
                    GetMockToken(TokenType.Numeric, "2"),
                    GetMockToken(TokenType.Numeric, "3")
                });

            return source.Object;
        }

        /// <summary>
        /// Gets a valid token list source comprising a pair containing two pairs of empty parentheses.
        /// </summary>
        /// <returns></returns>
        public static ITokenListSource GetNilBracketsTokenListSource()
        {
            var source = new Mock<ITokenListSource>();
            source.Setup(s => s.Get())
                .Returns(new List<IToken>
                {
                    GetMockToken(TokenType.OpeningParenthesis, "("),
                    GetMockToken(TokenType.OpeningParenthesis, "("),
                    GetMockToken(TokenType.ClosingParenthesis, ")"),
                    GetMockToken(TokenType.OpeningParenthesis, "("),
                    GetMockToken(TokenType.ClosingParenthesis, ")"),
                    GetMockToken(TokenType.ClosingParenthesis, ")")
                });

            return source.Object;
        }

        /// <summary>
        /// Gets an invalid token list source comprising a set of parentheses containing a dot followed by a numeric.
        /// </summary>
        /// <returns></returns>
        public static ITokenListSource GetHeadlessPairTokenListSource()
        {
            var source = new Mock<ITokenListSource>();
            source.Setup(s => s.Get())
                .Returns(new List<IToken>
                {
                    GetMockToken(TokenType.OpeningParenthesis, "("),
                    GetMockToken(TokenType.Dot, ".", 2, 1),
                    GetMockToken(TokenType.Numeric, "1"),
                    GetMockToken(TokenType.ClosingParenthesis, ")")
                });

            return source.Object;
        }

        /// <summary>
        /// Gets an invalid token list source comprising an illegal dotted triplet form.
        /// </summary>
        /// <returns></returns>
        public static ITokenListSource GetDottedTripletTokenListSource()
        {
            var source = new Mock<ITokenListSource>();
            source.Setup(s => s.Get())
                .Returns(new List<IToken>
                {
                    GetMockToken(TokenType.OpeningParenthesis, "("),
                    GetMockToken(TokenType.Numeric, "1"),
                    GetMockToken(TokenType.Dot, "."),
                    GetMockToken(TokenType.Numeric, "2"),
                    GetMockToken(TokenType.Dot, ".", 8, 1),
                    GetMockToken(TokenType.Numeric, "3"),
                    GetMockToken(TokenType.ClosingParenthesis, ")")
                });

            return source.Object;
        }

        /// <summary>
        /// Gets an invalid token list source comprising three atomic expressions outside a list structure.
        /// </summary>
        /// <returns></returns>
        public static ITokenListSource GetFreestandingExpressionsTokenListSource()
        {
            var source = new Mock<ITokenListSource>();
            source.Setup(s => s.Get())
                .Returns(new List<IToken>
                {
                    GetMockToken(TokenType.Numeric, "1", 1, 1),
                    GetMockToken(TokenType.Numeric, "2"),
                    GetMockToken(TokenType.Numeric, "3")
                });

            return source.Object;
        }

        /// <summary>
        /// Gets a valid token list source comprising an improper list denoted using implicit notation.
        /// </summary>
        /// <returns></returns>
        public static ITokenListSource GetImplicitDottedPairTokenListSource()
        {
            var source = new Mock<ITokenListSource>();
            source.Setup(s => s.Get())
                .Returns(new List<IToken>
                {
                    GetMockToken(TokenType.OpeningParenthesis, "("),
                    GetMockToken(TokenType.Numeric, "1"),
                    GetMockToken(TokenType.Numeric, "2"),
                    GetMockToken(TokenType.Dot, "."),
                    GetMockToken(TokenType.Numeric, "3"),
                    GetMockToken(TokenType.ClosingParenthesis, ")")
                });
            return source.Object;
        }
    }
}
