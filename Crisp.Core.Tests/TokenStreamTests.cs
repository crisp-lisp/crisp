using Microsoft.VisualStudio.TestTools.UnitTesting;

using Crisp.Core.Parsing;
using Crisp.Core.Tokenizing;

namespace Crisp.Core.Tests
{
    [TestClass]
    public class TokenStreamTests
    {
        [TestMethod]
        public void TestTokenStreamReadsSingleTokens()
        {
            /**
             * Description: The token stream should be capable of reading single tokens at
             * a time. This test ensures that this is the case.
             */

            // Tokenize input.
            const string sample = "a b c d e";
            var actual = TokenizerFactory.GetCrispTokenizer(true).Tokenize(sample);

            // Expected sequences.
            var expected = new[] {"a", "b", "c", "d", "e"};

            // Lists should be same length.
            Assert.AreEqual(actual.Count, expected.Length);

            // Test each token's sequence.
            var stream = new TokenStream(actual);
            foreach (var sequence in expected)
            {
                Assert.AreEqual(sequence, stream.Read().Sequence);
            }
        }

        [TestMethod]
        public void TestTokenStreamReadsAtomicExpressions()
        {
            /**
             * Description: The token stream should be capable of reading single atoms
             * at a time when asked to read an expression. This test ensures that this is
             * the case.
             */

            const string sample = "1 (2 3) (4 5)";
            var tokens = TokenizerFactory.GetCrispTokenizer().Tokenize(sample);

            var expected = new[] { "1" };

            var stream = new TokenStream(tokens);
            var actual = stream.ReadExpression();

            Assert.AreEqual(expected.Length, actual.Count);

            for (var i = 0; i < expected.Length; i++)
            {
                Assert.AreEqual(expected[i], actual[i].Sequence);
            }
        }

        [TestMethod]
        public void TestTokenStreamReadsListExpressions()
        {
            /**
             * Description: The token stream should be capable of reading entire lists
             * at a time when asked to read an expression. This test ensures that this
             * is the case.
             */

            const string sample = "(1 (2 3)) (4 5)";
            var tokens = TokenizerFactory.GetCrispTokenizer(true).Tokenize(sample);

            var expected = new[] { "(", "1", "(", "2", "3", ")", ")" };
            
            var stream = new TokenStream(tokens);
            var actual = stream.ReadExpression();
            
            Assert.AreEqual(expected.Length, actual.Count);

            for (var i = 0; i < expected.Length; i++)
            {
                Assert.AreEqual(expected[i], actual[i].Sequence);
            }
        }

        [TestMethod]
        public void TestTokenStreamReadsWholeInputListExpressions()
        {
            /**
             * Description: The token stream should be capable of reading the entire input
             * at once if the entire input consists of just one list expression. This test
             * ensures that this is the case.
             */

            const string sample = "(1 (2 3) 4 5)";
            var tokens = TokenizerFactory.GetCrispTokenizer(true).Tokenize(sample);

            var expected = new[] {"(", "1", "(", "2", "3", ")", "4", "5", ")"};
            
            var stream = new TokenStream(tokens);
            var actual = stream.ReadExpression();
            
            Assert.AreEqual(expected.Length, actual.Count);

            for (var i = 0; i < expected.Length; i++)
            {
                Assert.AreEqual(expected[i], actual[i].Sequence);
            }
        }
    }
}
