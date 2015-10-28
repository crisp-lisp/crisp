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
            // Tokenize input.
            const string sample = "a b c d e";
            var actual = TokenizerFactory.GetCrispTokenizer().Tokenize(sample);

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
            const string sample = "(1 (2 3)) (4 5)";
            var tokens = TokenizerFactory.GetCrispTokenizer().Tokenize(sample);

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
            const string sample = "(1 (2 3) 4 5)";
            var tokens = TokenizerFactory.GetCrispTokenizer().Tokenize(sample);

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
