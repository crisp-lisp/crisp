using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Crisp.Enums;

namespace Crisp.Tokenization.Tests
{
    [TestClass]
    public class TokenFilterFactoryTests
    {
        [TestMethod]
        public void TestGetCommentAndWhitespaceFilter()
        {
            /**
             * Description: The token filter factory should pass back a filter for comments and whitespace only, this 
             * test checks that this is the case.
             */

            // Use factory to create instance.
            var actual = TokenFilterFactory.GetCommentAndWhitespaceFilter();

            // Two token types should be filtered.
            Assert.AreEqual(2, actual.Filtered.Count());

            // These should be comments and whitespace.
            Assert.IsTrue(actual.Filtered.Contains(TokenType.Comment));
            Assert.IsTrue(actual.Filtered.Contains(TokenType.Whitespace));
        }
    }
}
