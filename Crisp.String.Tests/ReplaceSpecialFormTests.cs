using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;
using Ploeh.AutoFixture;

using Crisp.Shared;
using Crisp.Types;

namespace Crisp.String.Tests
{
    [TestClass]
    public class ReplaceSpecialFormTests
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
        public void ReplaceSpecialFormShouldProduceCorrectResult()
        {
            /**
             * Description: The replace special form should return a string atom with all occurrences of the second 
             * parameter replaced with the third. This test ensures that this is the case.
             */

            // Special form to test.
            var function = new ReplaceSpecialForm();

            // Compute answer.
            var subject = new StringAtom("the quick brown fox jumped over the other lazy fox");
            var term = new StringAtom("fox");
            var replacement = new StringAtom("raccoon");
            var args = new List<ISymbolicExpression> { subject, term, replacement }.ToProperList();
            var ans = function.Apply(args, _mockEvaluator);

            // We should have the correct string atom as a result.
            Assert.AreEqual(ans.Type, SymbolicExpressionType.String);
            Assert.AreEqual(subject.Value.Replace(term.Value, replacement.Value), ans.AsString().Value);
        }

        [TestMethod]
        public void ReplaceSpecialFormShouldTakeCorrectNumberOfParameters()
        {
            /**
             * Description: This special form only takes a certain number of parameters, this test ensures that an 
             * exception is thrown in case more than that amount is given.
             */

            var function = new ReplaceSpecialForm();

            var arg = _fixture.Create<StringAtom>();
            var correct = new List<ISymbolicExpression> { arg, arg, arg }.ToProperList();
            var incorrect = new List<ISymbolicExpression> { arg, arg }.ToProperList();

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
