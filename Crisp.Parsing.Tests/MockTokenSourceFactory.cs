using System.Collections.Generic;

using Moq;

using Crisp.Enums;
using Crisp.Interfaces.Tokenization;

namespace Crisp.Parsing.Tests
{
    public static class MockTokenSourceFactory
    {
        public static IToken GetMockToken(TokenType tokenType, string sequence, int column = -1, int line = -1)
        {
            var token = new Mock<IToken>();
            token.SetupGet(t => t.Type).Returns(tokenType);
            token.SetupGet(t => t.Sequence).Returns(sequence);
            token.SetupGet(t => t.Column).Returns(column);
            token.SetupGet(t => t.Line).Returns(line);
            return token.Object;
        }

        public static ITokenListSource GetProperListTokenSource()
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

        public static ITokenListSource GetImproperListTokenSource()
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

        public static ITokenListSource GetMismatchedBracketsTokenSource()
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

        public static ITokenListSource GetNilBracketsTokenSource()
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

        public static ITokenListSource GetHeadlessPairTokenSource()
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

        public static ITokenListSource GetDottedTripletTokenSource()
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

        public static ITokenListSource GetFreestandingExpressionsTokenSource()
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

        public static ITokenListSource GetImplicitDottedPairTokenSource()
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
