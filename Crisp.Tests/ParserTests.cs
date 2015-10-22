using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Crisp.Tokenizing;
using Crisp.Parsing;
using Crisp.Visualization;

namespace Crisp.Tests
{
    [TestClass]
    public class ParserTests
    {
        [TestMethod]
        public void ParserBuildsProperLists()
        {
            // Tokenize input to parse.
            var tokenizer = TokenizerFactory.GetCrispTokenizer();
            var tokens = tokenizer.Tokenize("(add 1 2 3 4.5 5)");

            // Parse tokens.
            var subject = new Parser();
            var actual = new LispSerializer().Serialize(subject.Parse(tokens));

            // A proper list should be built.
            var expected = "(add . (1 . (2 . (3 . (4.5 . (5 . NIL))))))";

            // Compare serialized list to expected.
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ParserShouldUseDotNotation()
        {
            // Tokenize input to parse.
            var tokenizer = TokenizerFactory.GetCrispTokenizer();
            var tokens = tokenizer.Tokenize("(let (add x y) (x . 3) (y . 5))");

            // Parse tokens.
            var subject = new Parser();
            var actual = new LispSerializer().Serialize(subject.Parse(tokens));

            // Dot notation should create improper listsfor binding expressions.
            var expected = "(let . ((add . (x . (y . NIL))) . ((x . 3) . ((y . 5) . NIL))))";

            // Compare serialized list to expected.
            Assert.AreEqual(expected, actual);
        }
    }
}
