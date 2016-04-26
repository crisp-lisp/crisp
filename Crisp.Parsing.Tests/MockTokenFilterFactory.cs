using System.Collections.Generic;
using Moq;

using Crisp.Interfaces.Tokenization;

namespace Crisp.Parsing.Tests
{
    /// <summary>
    /// A helper class for producing mock <see cref="ITokenFilter"/> instances.
    /// </summary>
    public static class MockTokenFilterFactory
    {
        /// <summary>
        /// Returns a mock token filter that does not filter any token types.
        /// </summary>
        /// <returns></returns>
        public static ITokenFilter GetMockTokenFilter()
        {
            var mockFilter = new Mock<ITokenFilter>();
            mockFilter.Setup(m => m.Filter(It.IsAny<IList<IToken>>()))
                .Returns((IList<IToken> l) => l); // Just return what we're given.

            return mockFilter.Object;
        }
    }
}
