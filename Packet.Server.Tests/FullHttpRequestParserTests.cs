using Microsoft.VisualStudio.TestTools.UnitTesting;

using DeepEqual.Syntax;
using Newtonsoft.Json;

namespace Packet.Server.Tests
{
    [TestClass]
    public class FullHttpRequestParserTests
    {
        [TestMethod]
        public void TestParseGetRequest()
        {
            /*
             * Description: The parser should handle simple well-formed HTTP/1.0 GET requests. This test checks that  
             * this is the case.
             */

            var subject = new FullHttpRequestParser(null);
            var actual = subject.Parse(SampleRawHttpRequestFactory.GetGetHttpRequest_1_0());

            // Check that parsing was successful.
            Assert.IsNotNull(actual, "Failed to parse request.");

            var expected = JsonConvert.DeserializeObject<FullHttpRequest>(
                Properties.Resources.ParsedHttpRequest_1_0_1,
                new JsonSerializerSettings {TypeNameHandling = TypeNameHandling.Auto});

            // Request fields should be correct.
            actual.ShouldDeepEqual(expected);
        }

        [TestMethod]
        public void TestParseHeadRequest()
        {
            /*
             * Description: The parser should handle simple well-formed HTTP/1.0 HEAD requests. This test checks that  
             * this is the case.
             */

            var subject = new FullHttpRequestParser(null);
            var actual = subject.Parse(SampleRawHttpRequestFactory.GetHeadHttpRequest_1_0());

            // Check that parsing was successful.
            Assert.IsNotNull(actual, "Failed to parse request.");

            var expected = JsonConvert.DeserializeObject<FullHttpRequest>(
                Properties.Resources.ParsedHttpRequest_1_0_2,
                new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });

            // Request fields should be correct.
            actual.ShouldDeepEqual(expected);
        }
    }
}
