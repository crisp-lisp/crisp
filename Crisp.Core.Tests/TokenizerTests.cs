using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Crisp.Core.Tokenizing;

namespace Crisp.Core.Tests
{
    [TestClass]
    public class TokenizerTests
    {
        [TestMethod]
        public void TestTokenizerTracksPositionProperly()
        {
            /**
             * Description: The tokenizer should track its position in the source code in
             * order to report on the position of errors. This test ensures that this tracking
             * is performed properly.
             */

            // Setup tokenizer.
            var subject = new Tokenizer();
            subject.Add(@"[a-z]", TokenType.None);

            // Tokenize input.
            const string sample = "a b  c   d    e\r\na b  c   d    e";
            var actual = subject.Tokenize(sample);

            // Expected output.
            var expected = new List<Token>()
            {
                new Token(TokenType.None, "a", 1, 1),
                new Token(TokenType.None, "b", 1, 3),
                new Token(TokenType.None, "c", 1, 6),
                new Token(TokenType.None, "d", 1, 10),
                new Token(TokenType.None, "e", 1, 15),
                new Token(TokenType.None, "a", 2, 1),
                new Token(TokenType.None, "b", 2, 3),
                new Token(TokenType.None, "c", 2, 6),
                new Token(TokenType.None, "d", 2, 10),
                new Token(TokenType.None, "e", 2, 15),
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
             * Description: The parser should throw an error for unexpected characters and
             * report the position of those characters accurately in the exception thrown.
             * This test checks that the error is thrown and that the reported position is
             * accurate.
             */

            var subject = new Tokenizer();
            subject.Add(@"[a-z]", TokenType.None);

            // Try to tokenize input.
            try
            {
                const string sample = "a b c\r\nd e 1 g";
                subject.Tokenize(sample);

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
             * Description: The tokenizer should capture the raw character sequences it
             * encounters in the tokens it generates. This test checks that this happens
             * accurately.
             */

            var subject = new Tokenizer();
            subject.Add(@"[a-z]+", TokenType.None);
            
            const string sample = "hello world";
            var actual = subject.Tokenize(sample);
            
            var expected = new List<Token>()
            {
                new Token(TokenType.None, "hello"),
                new Token(TokenType.None, "world"),
            };
            
            Assert.AreEqual(actual.Count, expected.Count);

            // Test each token's sequence.
            for (var i = 0; i < expected.Count; i++)
            {
                Assert.AreEqual(expected[i].Sequence, actual[i].Sequence);
            }
        }
    }
}
