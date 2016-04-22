using System.Collections.Generic;
using System.IO;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;
using Ploeh.AutoFixture;

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
            // Configure mock source file path provider.
            const string mockSourceFilePath = "C:\\fakepath\\source.csp";
            var mockSourceFilePathProvider = new Mock<ISourceFilePathProvider>();
            mockSourceFilePathProvider.Setup(obj => obj.Get())
                .Returns(mockSourceFilePath);

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

            // Create evaluator factory.
            var subject = new EvaluatorFactory(mockSourceFilePathProvider.Object,
                mockInterpreterDirectoryPathProvider.Object, mockSpecialFormLoader.Object);

            // Use factory to create evaluator.
            var actual = subject.Get();

            // Test that evaluator was created correctly.
            var mockSourceFileDirectory = Path.GetDirectoryName(mockSourceFilePath);
            Assert.AreEqual(mockSourceFileDirectory, actual.SourceFileDirectory);
            Assert.AreEqual(mockSourceFileDirectory, actual.WorkingDirectory);
            Assert.AreEqual(mockInterpreterDirectoryPath, actual.InterpreterDirectory);
        }
    }
}
