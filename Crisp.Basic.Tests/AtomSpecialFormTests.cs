using System.Collections.Generic;

using Crisp.Core;
using Crisp.Core.Evaluation;
using Crisp.Core.Types;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;
using Ploeh.AutoFixture;

namespace Crisp.Basic.Tests
{
    [TestClass]
    public class AtomSpecialFormTests
    {
        private static Fixture _fixture;
        private static IEvaluator _mockEvaluator;

        [ClassInitialize] 
        public static void Initialize(TestContext context)
        {
            _fixture = new Fixture();
            _fixture.Register<SymbolicExpression>(() => _fixture.Create<NumericAtom>());

            // Use a dummy evaluator that just returns what it's been given.
            var mockEvaluator = new Mock<IEvaluator>();
            mockEvaluator.Setup(m => m.Evaluate(It.IsAny<SymbolicExpression>()))
                .Returns((SymbolicExpression s) => s);
            _mockEvaluator = mockEvaluator.Object;
        }

        [TestMethod]
        public void AtomSpecialFormShouldDetectAtoms()
        {
            /**
             * Description: The atom special form should return a constant "t" if given an atom, 
             * otherwise it should return the constant "f". This test ensures that this behavior
             * is present.
             */

            // Special form to test.
            var function = new AtomSpecialForm();
            
            // Mock atomic and non-atomic args.
            var atomic = new List<SymbolicExpression> {_fixture.Create<NumericAtom>()}.ToProperList();
            var nonAtomic = new List<SymbolicExpression> {_fixture.Create<Pair>()}.ToProperList();

            // We should get "t" for atomic and "f" otherwise.
            Assert.AreEqual(function.Apply(atomic, _mockEvaluator).AsSymbol().Name, "t");
            Assert.AreEqual(function.Apply(nonAtomic, _mockEvaluator).AsSymbol().Name, "f");
        }

        [TestMethod]
        public void AtomSpecialFormShouldTakeCorrectNumberOfParameters()
        {
            /**
             * Description: This special form only takes a certain number of parameters, this
             * test ensures that an exception is thrown in case more than that amount is given.
             */

            var function = new AtomSpecialForm();
            
            var arg = _fixture.Create<NumericAtom>();
            var correct = new List<SymbolicExpression> {arg}.ToProperList();
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
