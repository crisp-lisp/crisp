using Moq;

using Crisp.Interfaces.Shared;

namespace Crisp.Tokenization.Tests
{
    /// <summary>
    /// A helper class for producing mock <see cref="ISourceCodeProvider"/> instances.
    /// </summary>
    public static class MockSourceCodeProviderFactory
    {
        /// <summary>
        /// Gets a mock source code provider that passes back the specified source code.
        /// </summary>
        /// <param name="source">The source code to pass back.</param>
        /// <returns></returns>
        public static ISourceCodeProvider GetMockSourceCodeProvider(string source)
        {
            var mock = new Mock<ISourceCodeProvider>();
            mock.Setup(obj => obj.Get())
                .Returns(source);
            return mock.Object;
        }
    }
}
