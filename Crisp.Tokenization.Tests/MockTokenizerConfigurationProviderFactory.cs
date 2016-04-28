using System.Collections.Generic;

using Crisp.Enums;
using Crisp.Interfaces.Tokenization;

using Moq;

namespace Crisp.Tokenization.Tests
{
    /// <summary>
    /// A helper class for producing mock <see cref="ITokenizerConfigurationProvider"/> instances.
    /// </summary>
    public static class MockTokenizerConfigurationProviderFactory
    {
        /// <summary>
        /// Passes back a tokenizer configuration provider for alphabetic symbols and whitespace only.
        /// </summary>
        /// <returns></returns>
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
