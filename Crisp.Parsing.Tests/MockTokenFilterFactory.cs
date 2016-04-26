using System.Collections.Generic;
using Moq;

using Crisp.Interfaces.Tokenization;

namespace Crisp.Parsing.Tests
{
    public static class MockTokenFilterFactory
    {
        public static ITokenFilter GetMockTokenFilter()
        {
            var mockFilter = new Mock<ITokenFilter>();
            mockFilter.Setup(m => m.Filter(It.IsAny<IList<IToken>>()))
                .Returns((IList<IToken> l) => l);
            return mockFilter.Object;
        }
    }
}
