using System.Collections.Generic;
using System.Linq;
using Crisp.Enums;
using Crisp.Interfaces;
using Crisp.Interfaces.Evaluation;
using Crisp.Interfaces.Types;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;
using Ploeh.AutoFixture;

using Crisp.Shared;
using Crisp.Types;

namespace Crisp.Basic.Tests
{
    [TestClass]
    public class LeqSpecialFormTests
    {
        private static Fixture _fixture;
        private static IEvaluator _mockEvaluator;

        [ClassInitialize] 
        public static void Initialize(TestContext context)
        {
            // Use a dummy evaluator that just returns what it's been given.
            var mockEvaluator = new Mock<IEvaluator>();
            mockEvaluator.Setup(m => m.Evaluate(It.IsAny<ISymbolicExpression>()))
                .Returns((ISymbolicExpression s) => s);
            _mockEvaluator = mockEvaluator.Object;

            // Register creation of expression types.
            _fixture = new Fixture();
            _fixture.Register(() => new Closure(
                _fixture.CreateMany<SymbolAtom>().ToList(), 
                _fixture.Create<StringAtom>(), 
                _mockEvaluator));
            _fixture.Register(() => new Lambda(
                _fixture.CreateMany<SymbolAtom>().ToList(),
                _fixture.Create<StringAtom>()));
            _fixture.Register(() => new Pair(
                _fixture.Create<StringAtom>(),
                _fixture.Create<StringAtom>()));
        }

        [TestMethod]
        public void LeqSpecialFormShouldWorkForBooleans()
        {
            /**
             * Description: For boolean inputs, the leq special form should return a boolean atom with a value of true 
             * if both of its arguments evaluate to the same value. Otherwise it should return a boolean atom with a 
             * value of false. This test checks that this is the case.
             */

            // Special form to test.
            var function = new LeqSpecialForm();
            
            // Compute result.
            var x = _fixture.Create<BooleanAtom>();
            var y = _fixture.Create<BooleanAtom>();
            var args = new List<ISymbolicExpression> {x, y}.ToProperList();
            var ans = function.Apply(args, _mockEvaluator);
            
            // We should have the correct boolean atom as a result.
            Assert.AreEqual(ans.Type, SymbolicExpressionType.Boolean);
            Assert.AreEqual(x.Value == y.Value, ans.AsBoolean().Value);
        }

        [TestMethod]
        public void LeqSpecialFormShouldAlwaysGiveFalseForClosures()
        {
            /**
             * Description: The leq special form should always return false for non-atomic expressions like closures,
             * even in a direct comparison of an expression to itself. This test checks that this is the case.
             */

            // Special form to test.
            var function = new LeqSpecialForm();

            // Compute result.
            var x = _fixture.Create<Closure>();
            var args = new List<ISymbolicExpression> {x, x}.ToProperList();
            var ans = function.Apply(args, _mockEvaluator);

            // We should have the correct boolean atom as a result.
            Assert.AreEqual(ans.Type, SymbolicExpressionType.Boolean);
            Assert.IsFalse(ans.AsBoolean().Value);
        }

        [TestMethod]
        public void LeqSpecialFormShouldWorkForConstants()
        {
            /**
             * Description: For constant inputs, the leq special form should return a boolean atom with a value of true 
             * if both of its arguments evaluate to the same value. Otherwise it should return a boolean atom with a 
             * value of false. This test checks that this is the case.
             */

            // Special form to test.
            var function = new LeqSpecialForm();

            // Compute result.
            var name = _fixture.Create<string>();
            var x = new ConstantAtom(name);
            var y = new ConstantAtom(name);
            var args = new List<ISymbolicExpression> {x, y}.ToProperList();
            var ans = function.Apply(args, _mockEvaluator);

            // We should have the correct boolean atom as a result.
            Assert.AreEqual(ans.Type, SymbolicExpressionType.Boolean);
            Assert.IsTrue(ans.AsBoolean().Value);
        }

        [TestMethod]
        public void LeqSpecialFormShouldAlwaysGiveFalseForLambdas()
        {
            /**
             * Description: The leq special form should always return false for non-atomic expressions like lambdas,
             * even in a direct comparison of an expression to itself. This test checks that this is the case.
             */

            // Special form to test.
            var function = new LeqSpecialForm();

            // Compute result.
            var x = _fixture.Create<Lambda>();
            var args = new List<ISymbolicExpression> {x, x}.ToProperList();
            var ans = function.Apply(args, _mockEvaluator);

            // We should have the correct boolean atom as a result.
            Assert.AreEqual(ans.Type, SymbolicExpressionType.Boolean);
            Assert.IsFalse(ans.AsBoolean().Value);
        }

        [TestMethod]
        public void LeqSpecialFormShouldWorkForNil()
        {
            /**
             * Description: For nil inputs, the leq special form should return a boolean atom with a value of true if 
             * both of its arguments evaluate to the same value. Otherwise it should return a boolean atom with a value 
             * of false. This test checks that this is the case.
             */

            // Special form to test.
            var function = new LeqSpecialForm();

            // Compute result.
            var x = _fixture.Create<Nil>();
            var y = _fixture.Create<Nil>();
            var args = new List<ISymbolicExpression> {x, y}.ToProperList();
            var ans = function.Apply(args, _mockEvaluator);

            // We should have the correct boolean atom as a result.
            Assert.AreEqual(ans.Type, SymbolicExpressionType.Boolean);
            Assert.IsTrue(ans.AsBoolean().Value);
        }

        [TestMethod]
        public void LeqSpecialFormShouldWorkForNumerics()
        {
            /**
             * Description: For numeric inputs, the leq special form should return a boolean atom with a value of true 
             * if its first argument evaluates to the less than or equal to its first argument. Otherwise it should 
             * return a boolean atom with a value of false. This test checks that this is the case.
             */

            // Special form to test.
            var function = new LeqSpecialForm();

            // Compute result.
            var x = new NumericAtom(_fixture.Create<double>());
            var y = new NumericAtom(_fixture.Create<double>());
            var args = new List<ISymbolicExpression> { x, y }.ToProperList();
            var ans = function.Apply(args, _mockEvaluator);

            // We should have the correct boolean atom as a result.
            Assert.AreEqual(ans.Type, SymbolicExpressionType.Boolean);
            Assert.AreEqual(x.Value <= y.Value, ans.AsBoolean().Value);
        }

        [TestMethod]
        public void LeqSpecialFormShouldAlwaysGiveFalseForPairs()
        {
            /**
             * Description: The leq special form should always return false for non-atomic expressions like pairs,
             * even in a direct comparison of an expression to itself. This test checks that this is the case.
             */

            // Special form to test.
            var function = new LeqSpecialForm();

            // Compute result.
            var x = _fixture.Create<Pair>();
            var args = new List<ISymbolicExpression> { x, x }.ToProperList();
            var ans = function.Apply(args, _mockEvaluator);

            // We should have the correct boolean atom as a result.
            Assert.AreEqual(ans.Type, SymbolicExpressionType.Boolean);
            Assert.IsFalse(ans.AsBoolean().Value);
        }

        [TestMethod]
        public void LeqSpecialFormShouldWorkForStrings()
        {
            /**
             * Description: For string inputs, the leq special form should return a boolean atom with a value of true 
             * if both of its arguments evaluate to the same value. Otherwise it should return a boolean atom with a 
             * value of false. This test checks that this is the case.
             */

            // Special form to test.
            var function = new LeqSpecialForm();

            // Compute result.
            var value = _fixture.Create<string>();
            var x = new StringAtom(value);
            var y = new StringAtom(value);
            var args = new List<ISymbolicExpression> { x, y }.ToProperList();
            var ans = function.Apply(args, _mockEvaluator);

            // We should have the correct boolean atom as a result.
            Assert.AreEqual(ans.Type, SymbolicExpressionType.Boolean);
            Assert.IsTrue(ans.AsBoolean().Value);
        }

        [TestMethod]
        public void LeqSpecialFormShouldWorkForSymbols()
        {
            /**
             * Description: For symbolic inputs, the leq special form should return a boolean atom with a value of true 
             * if both of its arguments evaluate to the same value. Otherwise it should return a boolean atom with a 
             * value of false. This test checks that this is the case.
             */

            // Special form to test.
            var function = new LeqSpecialForm();

            // Compute result.
            var name = _fixture.Create<string>();
            var x = new SymbolAtom(name);
            var y = new SymbolAtom(name);
            var args = new List<ISymbolicExpression> { x, y }.ToProperList();
            var ans = function.Apply(args, _mockEvaluator);

            // We should have the correct boolean atom as a result.
            Assert.AreEqual(ans.Type, SymbolicExpressionType.Boolean);
            Assert.IsTrue(ans.AsBoolean().Value);
        }

        [TestMethod]
        public void LeqSpecialFormShouldTakeCorrectNumberOfParameters()
        {
            /**
             * Description: This special form only takes a certain number of parameters, this test ensures that an 
             * exception is thrown in case more than that amount is given.
             */

            var function = new LeqSpecialForm();
            
            var arg = _fixture.Create<NumericAtom>();
            var correct = new List<ISymbolicExpression> {arg, arg}.ToProperList();
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
