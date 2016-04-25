using System.Collections.Generic;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;
using Ploeh.AutoFixture;

using Crisp.Interfaces;
using Crisp.Shared;

namespace Crisp.Evaluation.Tests
{
    [TestClass]
    public class EvaluatorFactoryTests
    {
        private readonly Fixture _fixture;

        public EvaluatorFactoryTests()
        {
            _fixture = new Fixture();
        }


        [TestMethod]
        public void TestGet()
        {
            // Configure mock source file directory path provider.
            const string mockSourceFileDirectoryPath = "C:\\fakepath\\source";
            var mockSourceFilePathProvider = new Mock<ISourceFileDirectoryPathProvider>();
            mockSourceFilePathProvider.Setup(obj => obj.Get())
                .Returns(mockSourceFileDirectoryPath);

            // Configure mock interpreter directory path provider.
            const string mockInterpreterDirectoryPath = "C:\\fakepath\\interpreter";
            var mockInterpreterDirectoryPathProvider = new Mock<IInterpreterDirectoryPathProvider>();
            mockInterpreterDirectoryPathProvider.Setup(obj => obj.Get())
                .Returns(mockInterpreterDirectoryPath);

            // Configure mock special form loader.
            var mockName = _fixture.Create<string>();
            var mockSpecialForm = Mock.Of<ISymbolicExpression>();
            var mockSpecialForms = new Dictionary<string, ISymbolicExpression>
            {
                {mockName, mockSpecialForm}
            };
            var mockSpecialFormLoader = new Mock<ISpecialFormLoader>();
            mockSpecialFormLoader.Setup(obj => obj.GetBindings())
                .Returns(mockSpecialForms);

            // Use factory to create evaluator.
            var subject = new EvaluatorFactory(mockSourceFilePathProvider.Object,
                mockInterpreterDirectoryPathProvider.Object, mockSpecialFormLoader.Object);
            var actual = subject.Get();

            // Test that evaluator was created correctly.
            Assert.AreEqual(mockSourceFileDirectoryPath, actual.SourceFileDirectory);
            Assert.AreEqual(mockSourceFileDirectoryPath, actual.WorkingDirectory);
            Assert.AreEqual(mockInterpreterDirectoryPath, actual.InterpreterDirectory);

            // Check that bindings were passed in properly.
            var binding = actual.Bindings.FirstOrDefault();
            Assert.IsNotNull(binding);
            Assert.AreEqual(mockName, binding.Name);
            Assert.AreEqual(mockSpecialForm, binding.Expression);
        }
    }
}
