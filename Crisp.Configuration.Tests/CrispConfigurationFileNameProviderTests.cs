using Microsoft.VisualStudio.TestTools.UnitTesting;

using Ploeh.AutoFixture;

namespace Crisp.Configuration.Tests
{
    [TestClass]
    public class CrispConfigurationFileNameProviderTests
    {
        private readonly Fixture _fixture;

        public CrispConfigurationFileNameProviderTests()
        {
            _fixture = new Fixture();
        }

        [TestMethod]
        public void TestGet()
        {
            // Pass string through provider.
            var expected = _fixture.Create<string>();
            var actual = new CrispConfigurationFileNameProvider(expected).Get();

            // We should get the same string back.
            Assert.AreEqual(expected, actual);
        }
    }
}
