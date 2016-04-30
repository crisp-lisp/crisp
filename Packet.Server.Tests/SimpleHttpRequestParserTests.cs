using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Packet.Enums;

namespace Packet.Server.Tests
{
    [TestClass]
    public class SimpleHttpRequestParserTests
    {
        [TestMethod]
        public void TestParseSimpleRequest()
        {
            var subject = new SimpleHttpRequestParser(null);
            var request = subject.Parse(SampleRawHttpRequestFactory.GetSimpleHttpRequest());

            Assert.IsNotNull(request, "Failed to parse request.");

            Assert.AreEqual(RequestType.Simple, request.RequestType);
            Assert.AreEqual(HttpMethod.Get, request.Method);
            Assert.AreEqual("/index.html", request.Url);
        }

        [TestMethod]
        public void TestParseSimpleRequestWithExtraData()
        {
            var subject = new SimpleHttpRequestParser(null);
            var request = subject.Parse(SampleRawHttpRequestFactory.GetSimpleHttpRequestWithExtraData());

            Assert.IsNotNull(request, "Failed to parse request.");

            Assert.AreEqual(RequestType.Simple, request.RequestType);
            Assert.AreEqual(HttpMethod.Get, request.Method);
            Assert.AreEqual("/index.html", request.Url);
        }
    }
}
