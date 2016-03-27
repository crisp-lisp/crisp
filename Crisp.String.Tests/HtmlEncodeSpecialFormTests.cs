using System.Collections.Generic;
using System.Web;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;
using Ploeh.AutoFixture;

using Crisp.Core;
using Crisp.Core.Evaluation;
using Crisp.Core.Types;

namespace Crisp.String.Tests
{
    [TestClass]
    public class HtmlEncodeSpecialFormTests
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
        public void HtmlEncodeSpecialFormShouldProduceCorrectResult()
        {
            /**
             * Description: The html-encode special form should return a a string atom that contains the given string
             * atom HTML-encoded. This test ensures that this is the case.
             */

            // Special form to test.
            var function = new HtmlEncodeSpecialForm();

            // Compute answer.
            var x = new StringAtom("<div></div>");
            var args = new List<SymbolicExpression> { x }.ToProperList();
            var ans = function.Apply(args, _mockEvaluator);

            // We should have the correct string atom as a result.
            Assert.AreEqual(ans.Type, SymbolicExpressionType.String);
            Assert.AreEqual(HttpUtility.HtmlEncode(x.Value), ans.AsString().Value);
        }

        [TestMethod]
        public void HtmlEncodeSpecialFormShouldTakeCorrectNumberOfParameters()
        {
            /**
             * Description: This special form only takes a certain number of parameters, this test ensures that an 
             * exception is thrown in case more than that amount is given.
             */

            var function = new HtmlEncodeSpecialForm();

            var arg = _fixture.Create<StringAtom>();
            var correct = new List<SymbolicExpression> { arg }.ToProperList();
            var incorrect = new List<SymbolicExpression> { arg, arg, arg }.ToProperList();

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
