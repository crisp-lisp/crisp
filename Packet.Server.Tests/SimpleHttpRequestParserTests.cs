using Microsoft.VisualStudio.TestTools.UnitTesting;

using Packet.Enums;

namespace Packet.Server.Tests
{
    [TestClass]
    public class SimpleHttpRequestParserTests
    {
        [TestMethod]
        public void TestParseRequest()
        {
            /*
             * Description: The parser should handle simple well-formed HTTP/0.9 requests. This test checks that this 
             * is the case.
             */

            var subject = new SimpleHttpRequestParser();
            var request = subject.Parse(SampleRawHttpRequestFactory.GetHttpRequest_0_9());

            // Check that parsing was successful.
            Assert.IsNotNull(request, "Failed to parse request.");

            // Request fields should be correct.
            Assert.AreEqual("HTTP/0.9", request.Version.ToString());
            Assert.AreEqual(HttpMethod.Get, request.Method);
            Assert.AreEqual("/index.html", request.Url);
        }

        [TestMethod]
        public void TestParseRequestWithExtraData()
        {
            /*
             * Description: The parser should ignore data after the URL for simple HTTP/0.9 requests. This test checks 
             * that this is the case.
             */

            var subject = new SimpleHttpRequestParser();
            var request = subject.Parse(SampleRawHttpRequestFactory.GetHttpRequestWithExtraData_0_9());

            Assert.IsNotNull(request, "Failed to parse request.");
            
            Assert.AreEqual("HTTP/0.9", request.Version.ToString());
            Assert.AreEqual(HttpMethod.Get, request.Method);
            Assert.AreEqual("/index.html", request.Url);
        }

        [TestMethod]
        public void TestParseMultilineRequestWithExtraData()
        {
            /*
             * Description: The parser should ignore data after the newline for simple HTTP/0.9 requests. This test 
             * checks that this is the case.
             */

            var subject = new SimpleHttpRequestParser();
            var request = subject.Parse(SampleRawHttpRequestFactory.GetMultilineHttpRequestWithExtraData_0_9());

            Assert.IsNotNull(request, "Failed to parse request.");

            Assert.AreEqual("HTTP/0.9", request.Version.ToString());
            Assert.AreEqual(HttpMethod.Get, request.Method);
            Assert.AreEqual("/index.html", request.Url);
        }
    }
}
