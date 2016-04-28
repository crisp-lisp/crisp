using System.Collections.Generic;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Crisp.Enums;
using Crisp.Interfaces.Tokenization;
using Crisp.Testing.Common;

namespace Crisp.Tokenization.Tests
{
    [TestClass]
    public class TokenizerTests
    {
        [TestMethod]
        public void TestTokenizerTracksPositionProperly()
        {
            /**
             * Description: The tokenizer should track its position in the source code in order to report on the 
             * position of errors. This test ensures that this tracking is performed properly.
             */

            // Source to tokenize.
            const string sample = "a b  c   d    e\r\na b  c   d    e";

            // Setup tokenizer and tokenize input.
            var subject = new Tokenizer(MockSourceCodeProviderFactory.GetMockSourceCodeProvider(sample),
                MockTokenizerConfigurationProviderFactory.GetAlphaTokenizerConfigurationProvider());
            var actual = subject.Get().Where(t => t.Type != TokenType.Whitespace).ToList();

            // Expected output.
            var expected = new List<IToken>
            {
                MockTokenFactory.GetMockToken(TokenType.Symbol, "a", 1, 1),
                MockTokenFactory.GetMockToken(TokenType.Symbol, "b", 3, 1),
                MockTokenFactory.GetMockToken(TokenType.Symbol, "c", 6, 1),
                MockTokenFactory.GetMockToken(TokenType.Symbol, "d", 10, 1),
                MockTokenFactory.GetMockToken(TokenType.Symbol, "e", 15, 1),
                MockTokenFactory.GetMockToken(TokenType.Symbol, "a", 1, 2),
                MockTokenFactory.GetMockToken(TokenType.Symbol, "b", 3, 2),
                MockTokenFactory.GetMockToken(TokenType.Symbol, "c", 6, 2),
                MockTokenFactory.GetMockToken(TokenType.Symbol, "d", 10, 2),
                MockTokenFactory.GetMockToken(TokenType.Symbol, "e", 15, 2),
            };

            // Token lists should be same length.
            Assert.AreEqual(actual.Count, expected.Count);

            // Test each token's position.
            for (var i = 0; i < expected.Count; i++)
            {
                Assert.AreEqual(expected[i].Column, actual[i].Column);
                Assert.AreEqual(expected[i].Line, actual[i].Line);
            }
        }

        [TestMethod]
        public void TestTokenizerThrowsErrorForUnexpectedCharacters()
        {
            /**
             * Description: The parser should throw an error for unexpected characters and report the position of those 
             * characters accurately in the exception thrown. This test checks that the error is thrown and that the 
             * reported position is accurate.
             */

            // Source to tokenize.
            const string sample = "a b c\r\nd e 1 g";

            // Setup tokenizer.
            var subject = new Tokenizer(MockSourceCodeProviderFactory.GetMockSourceCodeProvider(sample),
                MockTokenizerConfigurationProviderFactory.GetAlphaTokenizerConfigurationProvider());

            // Try to tokenize input.
            try
            {
                subject.Get();

                // We should have failed.
                Assert.Fail("Tokenizer should have failed to tokenize input.");
            }
            catch (TokenizationException ex)
            {
                // Error position should be reported correctly.
                Assert.AreEqual(5, ex.Column);
                Assert.AreEqual(2, ex.Line);
            }
        }

        [TestMethod]
        public void TestTokenizerCapturesSequences()
        {
            /**
             * Description: The tokenizer should capture the raw character sequences it encounters in the tokens it 
             * generates. This test checks that this happens accurately.
             */

            // Source to tokenize.
            const string sample = "hello world";

            // Setup tokenizer and tokenize input.
            var subject = new Tokenizer(MockSourceCodeProviderFactory.GetMockSourceCodeProvider(sample),
                MockTokenizerConfigurationProviderFactory.GetAlphaTokenizerConfigurationProvider());
            var actual = subject.Get().Where(t => t.Type != TokenType.Whitespace).ToList();

            // Expected output.
            var expected = new List<IToken>
            {
                MockTokenFactory.GetMockToken(TokenType.Symbol, "hello"),
                MockTokenFactory.GetMockToken(TokenType.Symbol, "world"),
            };

            // Token lists should be same length.
            Assert.AreEqual(actual.Count, expected.Count);

            // Test each token's sequence.
            for (var i = 0; i < expected.Count; i++)
            {
                Assert.AreEqual(expected[i].Sequence, actual[i].Sequence);
            }
        }
    }
}
