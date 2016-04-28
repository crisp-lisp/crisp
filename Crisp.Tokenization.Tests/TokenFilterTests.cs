using Microsoft.VisualStudio.TestTools.UnitTesting;

using Crisp.Enums;
using Crisp.Testing.Common;

namespace Crisp.Tokenization.Tests
{
    [TestClass]
    public class TokenFilterTests
    {
        [TestMethod]
        public void TestFilter()
        {
            /**
             * Description: The token filter should remove all tokens of a given set of types from a given token list,
             * this test checks that this is the case.
             */

            // Remove comments and whitespace.
            var subject = new TokenFilter(new[] {TokenType.Comment, TokenType.Whitespace});

            // Filter expression list with comments and whitespace.
            var actual = subject.Filter(MockTokenListFactory.GetCommentedEntireListExpressionTokenList());

            // Get expression list without comments and whitespace.
            var expected = MockTokenListFactory.GetEntireListExpressionTokenList();
            
            // Both lists should be identical.
            Assert.AreEqual(expected.Count, actual.Count);

            for (var i = 0; i < actual.Count; i++)
            {
                Assert.AreEqual(expected[i].Type, actual[i].Type);
            }
        }
    }
}
