using Moq;

using Crisp.Enums;
using Crisp.Interfaces.Tokenization;

namespace Crisp.Testing.Common
{
    /// <summary>
    /// A helper class for producing mock <see cref="IToken"/> instances.
    /// </summary>
    public static class MockTokenFactory
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
    }
}
