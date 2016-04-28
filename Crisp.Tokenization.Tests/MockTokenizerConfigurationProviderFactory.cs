using System;
using System.Collections.Generic;

using Crisp.Enums;
using Crisp.Interfaces.Tokenization;

using Moq;

namespace Crisp.Tokenization.Tests
{
    public static class MockTokenizerConfigurationProviderFactory
    {
        public static ITokenizerConfigurationProvider GetAlphaTokenizerConfigurationProvider()
        {
            var mock = new Mock<ITokenizerConfigurationProvider>();
            mock.Setup(obj => obj.Get())
                .Returns(new Dictionary<string, TokenType>
                {
                    {@"[a-z]+", TokenType.Symbol},
                    {"\\s+", TokenType.Whitespace}
                });
            return mock.Object;
        }
    }
}
