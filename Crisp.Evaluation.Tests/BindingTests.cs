using Crisp.Interfaces;
using Crisp.Interfaces.Evaluation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;
using Ploeh.AutoFixture;

using Crisp.Shared;

namespace Crisp.Evaluation.Tests
{
    [TestClass]
    public class BindingTests
    {
        private readonly Fixture _fixture;

        public BindingTests()
        {
            _fixture = new Fixture();
        }

        [TestMethod]
        public void TestName()
        {
            // Create binding.
            var name = _fixture.Create<string>();
            var subject = new Binding(name, Mock.Of<ISymbolicExpression>(), Mock.Of<IEvaluator>());

            // Name property should be assigned.
            Assert.AreEqual(name, subject.Name);
        }

        [TestMethod]
        public void TestValue()
        {
            // Set up mock evaluator.
            var mockEvaluatorResult = Mock.Of<ISymbolicExpression>();
            var mockEvaluator = new Mock<IEvaluator>();
            mockEvaluator.Setup(obj => obj.Evaluate(It.IsAny<ISymbolicExpression>()))
                .Returns(mockEvaluatorResult);

            // Create binding.
            var subject = new Binding(_fixture.Create<string>(), mockEvaluatorResult, mockEvaluator.Object);

            // Get value.
            var value = subject.Value;

            // We should get the mock evaluator result back.
            Assert.AreEqual(mockEvaluatorResult, value);

            // The evaluator should have been used for lazy evaluation.
            mockEvaluator.Verify(obj => obj.Evaluate(mockEvaluatorResult));
        }
    }
}
