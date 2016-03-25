using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;
using Ploeh.AutoFixture;

using Crisp.Core;
using Crisp.Core.Evaluation;
using Crisp.Core.Types;

namespace Crisp.Basic.Tests
{
    [TestClass]
    public class IfSpecialFormTests
    {
        private static Fixture _fixture;
        private static IEvaluator _mockEvaluator;

        [ClassInitialize]
        public static void Initialize(TestContext context)
        {
            _fixture = new Fixture();

            // Use a dummy evaluator that just returns what it's been given.
            var mockEvaluator = new Mock<IEvaluator>();
            mockEvaluator.Setup(m => m.Evaluate(It.IsAny<SymbolicExpression>()))
                .Returns((SymbolicExpression s) => s);
            _mockEvaluator = mockEvaluator.Object;
        }

        [TestMethod]
        public void IfSpecialFormShouldProduceCorrectResult()
        {
            /**
             * Description: The if special form should return its second argument if its first argument evaluates to
             * true, otherwise it should return its third argument. This test ensures that this is the case.
             */

            // Special form to test.
            var function = new IfSpecialForm();

            // Compute answer.
            var t = new BooleanAtom(true);
            var f = new BooleanAtom(false);
            var args = new List<SymbolicExpression> { t, t, f }.ToProperList();
            var ans = function.Apply(args, _mockEvaluator);

            // We should have the correct numeric atom as a result.
            Assert.AreEqual(ans.Type, SymbolicExpressionType.Boolean);
            Assert.IsTrue(ans.AsBoolean().Value);
        }

        [TestMethod]
        public void IfSpecialFormShouldTakeCorrectNumberOfParameters()
        {
            /**
             * Description: This special form only takes a certain number of parameters, this test ensures that an 
             * exception is thrown in case more than that amount is given.
             */

            var function = new IfSpecialForm();

            var arg = _fixture.Create<BooleanAtom>();
            var correct = new List<SymbolicExpression> { arg, arg, arg }.ToProperList();
            var incorrect = new List<SymbolicExpression> { arg, arg }.ToProperList();

            function.Apply(correct, _mockEvaluator);

            try
            {
                function.Apply(incorrect, _mockEvaluator);

                // We should have failed.
                Assert.Fail("Exception should have been thrown for wrong number of arguments.");
            }
            catch (RuntimeException) { }
        }
    }
}
