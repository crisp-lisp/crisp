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
            var subject = new TokenFilter(new[] {TokenType.Comment, TokenType.Whitespace});

            var actual = subject.Filter(MockTokenListFactory.GetCommentedEntireListExpressionTokenList());
            var expected = MockTokenListFactory.GetEntireListExpressionTokenList();
            
            Assert.AreEqual(expected.Count, actual.Count);

            for (var i = 0; i < actual.Count; i++)
            {
                Assert.AreEqual(expected[i].Type, actual[i].Type);
            }
        }
    }
}
