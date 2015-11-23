using System.Collections.Generic;

using Crisp.Core;
using Crisp.Core.Evaluation;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;
using Ploeh.AutoFixture;

namespace Crisp.Basic.Tests
{
    [TestClass]
    public class MulSpecialFormTests
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
        public void MulSpecialFormShouldProduceCorrectTotal()
        {
            /**
             * Description: The mul special form should return a numeric atom that contains
             * the product of two given numeric atoms as its value. This test ensures that this is
             * the case.
             */

            // Special form to test.
            var function = new MulSpecialForm();
            
            // Compute answer.
            var x = _fixture.Create<NumericAtom>();
            var y = _fixture.Create<NumericAtom>();
            var args = new List<SymbolicExpression> { x, y }.ToProperList();
            var ans = function.Apply(args, _mockEvaluator);
            
            // We should have the correct numeric atom as a result.
            Assert.AreEqual(ans.Type, SymbolicExpressionType.Numeric);
            Assert.AreEqual(x.Value * y.Value, ans.AsNumeric().Value);
        }

        [TestMethod]
        public void MulSpecialFormShouldTakeCorrectNumberOfParameters()
        {
            /**
             * Description: This special form only takes a certain number of parameters, this
             * test ensures that an exception is thrown in case more than that amount is given.
             */

            var function = new MulSpecialForm();
            
            var arg = _fixture.Create<NumericAtom>();
            var correct = new List<SymbolicExpression> {arg, arg}.ToProperList();
            var incorrect = new List<SymbolicExpression> {arg, arg, arg}.ToProperList();

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
