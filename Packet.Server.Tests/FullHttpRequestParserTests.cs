using System;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Packet.Enums;

namespace Packet.Server.Tests
{
    [TestClass]
    public class FullHttpRequestParserTests
    {
        [TestMethod]
        public void TestParseGetRequest()
        {
            /*
             * Description: The parser should handle simple well-formed HTTP/1.0 GET requests. This test checks that this 
             * is the case.
             */

            var subject = new FullHttpRequestParser(null);
            var request = subject.Parse(new ASCIIEncoding().GetBytes(Properties.Resources.SampleHttpRequest_1_0_1));

            // Check that parsing was successful.
            Assert.IsNotNull(request, "Failed to parse request.");

            // Request fields should be correct.
            Assert.AreEqual("HTTP/1.0", request.Version.ToString());
            Assert.AreEqual(HttpMethod.Get, request.Method);
            Assert.AreEqual("/4848", request.Url);
        }
    }
}
