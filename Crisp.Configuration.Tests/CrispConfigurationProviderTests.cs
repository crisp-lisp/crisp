using Crisp.Interfaces;
using Crisp.Interfaces.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using DeepEqual.Syntax;
using Moq;
using Newtonsoft.Json;

using Crisp.Shared;

namespace Crisp.Configuration.Tests
{
    [TestClass]
    public class CrispConfigurationProviderTests
    {
        private readonly IRawCrispConfigurationProvider _mockRawCrispConfigurationProvider;

        public CrispConfigurationProviderTests()
        {
            // Set up mock raw configuration provider.
            var mockRawConfigProvider = new Mock<IRawCrispConfigurationProvider>();
            mockRawConfigProvider.Setup(obj => obj.Get())
                .Returns(Properties.Resources.DummyCrispConfiguration);
            _mockRawCrispConfigurationProvider = mockRawConfigProvider.Object;
        }

        [TestMethod]
        public void TestGet()
        {
            // Load config manually and automatically.
            var expected = JsonConvert.DeserializeObject<CrispConfiguration>(Properties.Resources
                .DummyCrispConfiguration);
            var actual = new CrispConfigurationProvider(_mockRawCrispConfigurationProvider).Get();

            // Objects should deeply equal each other.
            actual.ShouldDeepEqual(expected);
        }
    }
}
