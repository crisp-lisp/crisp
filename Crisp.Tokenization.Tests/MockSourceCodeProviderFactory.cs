using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Moq;

using Crisp.Interfaces.Parsing;
using Crisp.Interfaces.Shared;

namespace Crisp.Tokenization.Tests
{
    public static class MockSourceCodeProviderFactory
    {
        public static ISourceCodeProvider GetMockSourceCodeProvider(string source)
        {
            var mock = new Mock<ISourceCodeProvider>();
            mock.Setup(obj => obj.Get())
                .Returns(source);
            return mock.Object;
        }
    }
}
