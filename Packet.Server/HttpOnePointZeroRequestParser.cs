using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

using Packet.Enums;
using Packet.Interfaces.Server;

namespace Packet.Server
{
    /// <summary>
    /// Represents a HTTP request parser that will parse HTTP/1.0 full-format requests. 
    /// </summary>
    public class HttpOnePointZeroRequestParser : HttpRequestParser
    {
        private static Regex _requestLineRegex;

        private static Regex _headerLineRegex;

        public HttpOnePointZeroRequestParser(IHttpRequestParser successor) : base(successor)
        {
            _requestLineRegex = new Regex("(?i)(\\S+?) (\\S+?) HTTP\\/1\\.0");
            _headerLineRegex = new Regex("^(.+?): (.+?)$");
        }

        protected override IHttpRequest AttemptParse(byte[] request)
        {
            using (var memoryStream = new MemoryStream(request))
            {
                using (var streamReader = new StreamReader(memoryStream))
                {
                    var requestLine = streamReader.ReadLine();

                    if (requestLine == null)
                    {
                        return null; // Request line was null, can't continue parsing.
                    }

                    // Validate request line format.
                    var match = _requestLineRegex.Match(requestLine);
                    if (!match.Success)
                    {
                        return null;
                    }

                    // Grab method from capturing group.
                    var method = HttpMethodConverter.Parse(match.Groups[1].Value);
                    if (method == HttpMethod.Unsupported)
                    {
                        return null; // Invalid method.
                    }

                    // Grab URL from capturing group.
                    var url = match.Groups[2].Value;

                    // Validate URL.
                    if (!ValidateUrl(url))
                    {
                        return null;
                    }

                    var headerBuffer = new Dictionary<string, string>();
                    string buffer;
                    while (!string.IsNullOrWhiteSpace(buffer = streamReader.ReadLine()))
                    {
                        var headerMatch = _headerLineRegex.Match(buffer);
                        if (!headerMatch.Success)
                        {
                            return null;
                        }
                        headerBuffer.Add(headerMatch.Groups[1].Value, headerMatch.Groups[2].Value);
                    }

                    return new HttpOnePointZeroRequest
                    {
                        Headers = headerBuffer,
                        Method = method,
                        Url = url,
                    };
                }
            }
        }
    }
}
