using System.Collections.Generic;

using Moq;

using Crisp.Enums;
using Crisp.Interfaces.Tokenization;

namespace Crisp.Testing.Common
{
    /// <summary>
    /// A helper class for producing mock <see cref="ITokenListSource"/> instances.
    /// </summary>
    public static class MockTokenListSourceFactory
    {
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
                    MockTokenFactory.GetMockToken(TokenType.OpeningParenthesis, "("),
                    MockTokenFactory.GetMockToken(TokenType.Numeric, "1"),
                    MockTokenFactory.GetMockToken(TokenType.Numeric, "2"),
                    MockTokenFactory.GetMockToken(TokenType.Numeric, "3"),
                    MockTokenFactory.GetMockToken(TokenType.ClosingParenthesis, ")")
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
                    MockTokenFactory.GetMockToken(TokenType.OpeningParenthesis, "("),
                    MockTokenFactory.GetMockToken(TokenType.Numeric, "1"),
                    MockTokenFactory.GetMockToken(TokenType.Numeric, "2"),
                    MockTokenFactory.GetMockToken(TokenType.Numeric, "3"),
                    MockTokenFactory.GetMockToken(TokenType.Dot, "."),
                    MockTokenFactory.GetMockToken(TokenType.Numeric, "4"),
                    MockTokenFactory.GetMockToken(TokenType.ClosingParenthesis, ")")
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
                    MockTokenFactory.GetMockToken(TokenType.OpeningParenthesis, "(", 1, 1),
                    MockTokenFactory.GetMockToken(TokenType.Numeric, "1"),
                    MockTokenFactory.GetMockToken(TokenType.Numeric, "2"),
                    MockTokenFactory.GetMockToken(TokenType.Numeric, "3")
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
                    MockTokenFactory.GetMockToken(TokenType.OpeningParenthesis, "("),
                    MockTokenFactory.GetMockToken(TokenType.OpeningParenthesis, "("),
                    MockTokenFactory.GetMockToken(TokenType.ClosingParenthesis, ")"),
                    MockTokenFactory.GetMockToken(TokenType.OpeningParenthesis, "("),
                    MockTokenFactory.GetMockToken(TokenType.ClosingParenthesis, ")"),
                    MockTokenFactory.GetMockToken(TokenType.ClosingParenthesis, ")")
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
                    MockTokenFactory.GetMockToken(TokenType.OpeningParenthesis, "("),
                    MockTokenFactory.GetMockToken(TokenType.Dot, ".", 2, 1),
                    MockTokenFactory.GetMockToken(TokenType.Numeric, "1"),
                    MockTokenFactory.GetMockToken(TokenType.ClosingParenthesis, ")")
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
                    MockTokenFactory.GetMockToken(TokenType.OpeningParenthesis, "("),
                    MockTokenFactory.GetMockToken(TokenType.Numeric, "1"),
                    MockTokenFactory.GetMockToken(TokenType.Dot, "."),
                    MockTokenFactory.GetMockToken(TokenType.Numeric, "2"),
                    MockTokenFactory.GetMockToken(TokenType.Dot, ".", 8, 1),
                    MockTokenFactory.GetMockToken(TokenType.Numeric, "3"),
                    MockTokenFactory.GetMockToken(TokenType.ClosingParenthesis, ")")
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
                    MockTokenFactory.GetMockToken(TokenType.Numeric, "1", 1, 1),
                    MockTokenFactory.GetMockToken(TokenType.Numeric, "2"),
                    MockTokenFactory.GetMockToken(TokenType.Numeric, "3")
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
                    MockTokenFactory.GetMockToken(TokenType.OpeningParenthesis, "("),
                    MockTokenFactory.GetMockToken(TokenType.Numeric, "1"),
                    MockTokenFactory.GetMockToken(TokenType.Numeric, "2"),
                    MockTokenFactory.GetMockToken(TokenType.Dot, "."),
                    MockTokenFactory.GetMockToken(TokenType.Numeric, "3"),
                    MockTokenFactory.GetMockToken(TokenType.ClosingParenthesis, ")")
                });
            return source.Object;
        }
    }
}
