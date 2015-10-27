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
            /**
             * Description: 
             */ 

            var tokenizer = TokenizerFactory.GetCrispTokenizer();
            const string sample = "(add 1 2 3 4.5 5)";
            var tokens = tokenizer.Tokenize(sample);
            
            var subject = new Parser();
            var actual = new LispSerializer().Serialize(subject.CreateExpressionTree(tokens));
            
            const string expected = "(add . (1 . (2 . (3 . (4.5 . (5 . NIL))))))";
            
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ParserShouldUseDotNotation()
        {
            /**
             * Description: Crisp supports dot notation for creating improper lists. This test
             * checks that dot notation is supported as required.
             */

            var tokenizer = TokenizerFactory.GetCrispTokenizer();
            const string sample = "(let (add x y) (x . 3) (y . 5))";
            var tokens = tokenizer.Tokenize(sample);
            
            var subject = new Parser();
            var actual = new LispSerializer().Serialize(subject.CreateExpressionTree(tokens));
            
            const string expected = "(let . ((add . (x . (y . NIL))) . ((x . 3) . ((y . 5) . NIL))))";
            
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ParserShouldFailOnMismatchedBrackets()
        {
            /**
             * Description: Mismatched brackets should cause the parser to fail for obvious
             * reasons. This test checks that a failure takes place as expected.
             */

            var tokenizer = TokenizerFactory.GetCrispTokenizer();
            const string sample = "((add 1 2)";
            var tokens = tokenizer.Tokenize(sample);
            
            var subject = new Parser();
            try
            {
                subject.CreateExpressionTree(tokens);
                Assert.Fail("Parser should have detected mismatched brackets.");
            }
            catch (ParsingException ex)
            {
                Assert.AreEqual(1, ex.Line);
                Assert.AreEqual(1, ex.Column);
            }
        }

        [TestMethod]
        public void ParserShouldTurnEmptyBracketsIntoNil()
        {
            /**
             * Description: Empty brackets are analogous to nil, so they are substituted out for 
             * the nil special symbol during parsing. This test checks that substitution has taken
             * place.
             */

            var tokenizer = TokenizerFactory.GetCrispTokenizer();
            const string sample = "(() ())";
            var tokens = tokenizer.Tokenize(sample);
            
            var subject = new Parser();
            var actual = new LispSerializer().Serialize(subject.CreateExpressionTree(tokens));
            
            const string expected = "(NIL . (NIL . NIL))";
            
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ParserShouldFailOnUnexpectedLeadingDot()
        {
            /**
             * Description: If the head part of a dotted pair is omitted, parsing should 
             * fail. This test checks for that failure.
             */

            var tokenizer = TokenizerFactory.GetCrispTokenizer();
            const string sample = "(. 1)";
            var tokens = tokenizer.Tokenize(sample);
            
            var subject = new Parser();
            try
            {
                subject.CreateExpressionTree(tokens);
                Assert.Fail("Parser should have detected invalid leading dot.");
            }
            catch (ParsingException ex)
            {
                Assert.AreEqual(1, ex.Line);
                Assert.AreEqual(2, ex.Column);
            }
        }

        [TestMethod]
        public void ParserShouldFailOnTooManyElementsInDottedPair()
        {
            /**
             * Description: If the parser encounters a 'dotted pair' with more than two
             * elements, it should fail. This test checks for that failure.
             */ 

            var tokenizer = TokenizerFactory.GetCrispTokenizer();
            const string sample = "(1 . 2 . 3)";
            var tokens = tokenizer.Tokenize(sample);

            var subject = new Parser();
            try
            {
                subject.CreateExpressionTree(tokens);
                Assert.Fail("Parser should have detected too many elements in dotted pair.");
            }
            catch (ParsingException ex)
            {
                Assert.AreEqual(1, ex.Line);
                Assert.AreEqual(8, ex.Column);
            }
        }

        [TestMethod]
        public void ParserShouldFailOnFreestandingExpressions()
        {
            /**
             * Description: If the parser is provided with a naked array of expressions with no
             * enclosing brackets, this can't be evaluated and parsing should fail. This
             * test checks for that failure.
             */

            var tokenizer = TokenizerFactory.GetCrispTokenizer();
            const string sample = "a b c";
            var tokens = tokenizer.Tokenize(sample);

            var subject = new Parser();
            try
            {
                subject.CreateExpressionTree(tokens);
                Assert.Fail("Parser should have detected freestanding expressions.");
            }
            catch (ParsingException ex)
            {
                Assert.AreEqual(1, ex.Line);
                Assert.AreEqual(1, ex.Column);
            }
        }
        
        [TestMethod]
        public void ParserShouldNotFailOnImplicitDottedPair()
        {
            /**
             * Description: Crisp permits a dot between the last two elements in a list literal 
             * to form an improper list. This unit test ensures that the parser doesn't choke.
             */

            var tokenizer = TokenizerFactory.GetCrispTokenizer();
            const string sample = "(1 2 . 3)";
            var tokens = tokenizer.Tokenize(sample);

            var subject = new Parser();
            var actual = new LispSerializer().Serialize(subject.CreateExpressionTree(tokens));
            
            const string expected = "(1 . (2 . 3))";

            Assert.AreEqual(expected, actual);
        }
    }
}
