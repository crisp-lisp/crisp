using System;
using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;
using Ploeh.AutoFixture;

using Crisp.Shared;
using Crisp.Types;

namespace Crisp.String.Tests
{
    [TestClass]
    public class SplitSpecialFormTests
    {
        private static Fixture _fixture;
        private static IEvaluator _mockEvaluator;

        [ClassInitialize]
        public static void Initialize(TestContext context)
        {
            _fixture = new Fixture();

            // Use a dummy evaluator that just returns what it's been given.
            var mockEvaluator = new Mock<IEvaluator>();
            mockEvaluator.Setup(m => m.Evaluate(It.IsAny<ISymbolicExpression>()))
                .Returns((ISymbolicExpression s) => s);
            _mockEvaluator = mockEvaluator.Object;
        }

        [TestMethod]
        public void SplitSpecialFormShouldProduceCorrectResult()
        {
            /**
             * Description: The split special form should return a list of string atoms that consists of the given
             * string atom split along occurrences of another given string atom. This test ensures that this is the
             * case.
             */

            // Special form to test.
            var function = new SplitSpecialForm();

            // Compute answer.
            var subject = new StringAtom("test:splittable:string");
            var delimiter = new StringAtom(":");
            var args = new List<ISymbolicExpression> { subject, delimiter }.ToProperList();
            var ans = function.Apply(args, _mockEvaluator);

            // We should have a pair (list) as a result.
            Assert.AreEqual(ans.Type, SymbolicExpressionType.Pair);

            // All members should be equal.
            var expected = subject.Value.Split(Convert.ToChar(delimiter.Value));
            var actual = ans.AsPair().Expand();
            Assert.AreEqual(expected.Length, actual.Count);
            for (var i = 0; i < expected.Length; i++)
            {
                Assert.AreEqual(expected[i], actual[i].AsString().Value);
            }
        }

        [TestMethod]
        public void SplitSpecialFormShouldTakeCorrectNumberOfParameters()
        {
            /**
             * Description: This special form only takes a certain number of parameters, this test ensures that an 
             * exception is thrown in case more than that amount is given.
             */

            var function = new SplitSpecialForm();

            var arg = _fixture.Create<StringAtom>();
            var correct = new List<ISymbolicExpression> { arg, arg }.ToProperList();
            var incorrect = new List<ISymbolicExpression> { arg, arg, arg }.ToProperList();

            function.Apply(correct, _mockEvaluator);

            try
            {
                function.Apply(incorrect, _mockEvaluator);

                // We should have failed.
                Assert.Fail("Exception should have been thrown for wrong number of arguments.");
            }
            catch (FunctionApplicationException) { }
        }
    }
}
