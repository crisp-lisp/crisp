using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Crisp.Parsing.Tests
{
    [TestClass]
    public class TokenStreamTests
    {
        [TestMethod]
        public void TestTokenStreamReadsSingleTokens()
        {
            /**
             * Description: The token stream should be capable of reading one token at a time. This test ensures that 
             * this is the case.
             */
             
            // Expected sequences.
            var expected = new[] {"a", "b", "c", "d", "e"};

            // Test each token's sequence.
            var stream = new TokenStream(MockTokenListFactory.GetSingleTokenList());
            foreach (var sequence in expected)
            {
                Assert.AreEqual(sequence, stream.Read().Sequence);
            }
        }

        [TestMethod]
        public void TestTokenStreamReadsAtomicExpressions()
        {
            /**
             * Description: The token stream should be capable of reading single atoms at a time when asked to read an 
             * expression. This test ensures that this is the case.
             */

            var expected = new[] { "1" };

            var stream = new TokenStream(MockTokenListFactory.GetMixedAtomicityTokenList());
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
             * Description: The token stream should be capable of reading entire lists at a time when asked to read an 
             * expression. This test ensures that this is the case.
             */

            var expected = new[] { "(", "1", "(", "2", "3", ")", ")" };
            
            var stream = new TokenStream(MockTokenListFactory.GetListExpressionTokenList());
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
             * Description: The token stream should be capable of reading the entire input at once if the entire input 
             * consists of just one list expression. This test ensures that this is the case.
             */

            var expected = new[] {"(", "1", "(", "2", "3", ")", "4", "5", ")"};
            
            var stream = new TokenStream(MockTokenListFactory.GetEntireListExpressionTokenList());
            var actual = stream.ReadExpression();
            
            Assert.AreEqual(expected.Length, actual.Count);

            for (var i = 0; i < expected.Length; i++)
            {
                Assert.AreEqual(expected[i], actual[i].Sequence);
            }
        }
    }
}
