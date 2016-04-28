using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;
using Ploeh.AutoFixture;

using Crisp.Interfaces.Evaluation;
using Crisp.Interfaces.Types;
using Crisp.Types;

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
            _fixture.Register<ISymbolicExpression>(() => _fixture.Create<NumericAtom>());

            // Use a dummy evaluator that just returns what it's been given.
            var mockEvaluator = new Mock<IEvaluator>();
            mockEvaluator.Setup(m => m.Evaluate(It.IsAny<ISymbolicExpression>()))
                .Returns((ISymbolicExpression s) => s);
            _mockEvaluator = mockEvaluator.Object;
        }

        [TestMethod]
        public void AtomSpecialFormShouldDetectAtoms()
        {
            /**
             * Description: The atom special form should return a true boolean atom if given an atom, otherwise it 
             * should return a true boolean atom. This test ensures that this behavior is present.
             */

            // Special form to test.
            var function = new AtomSpecialForm();
            
            // Mock atomic and non-atomic args.
            var atomic = new List<ISymbolicExpression> {_fixture.Create<NumericAtom>()}.ToProperList();
            var nonAtomic = new List<ISymbolicExpression> {_fixture.Create<Pair>()}.ToProperList();

            // We should get a true boolean atom for atomic and a false boolean atom otherwise.
            Assert.AreEqual(function.Apply(atomic, _mockEvaluator), new BooleanAtom(true));
            Assert.AreEqual(function.Apply(nonAtomic, _mockEvaluator), new BooleanAtom(false));
        }

        [TestMethod]
        public void AtomSpecialFormShouldTakeCorrectNumberOfParameters()
        {
            /**
             * Description: This special form only takes a certain number of parameters, this test ensures that an 
             * exception is thrown in case more than that amount is given.
             */

            var function = new AtomSpecialForm();
            
            var arg = _fixture.Create<NumericAtom>();
            var correct = new List<ISymbolicExpression> {arg}.ToProperList();
            var incorrect = new List<ISymbolicExpression> {arg, arg, arg}.ToProperList();

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
