using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

using Packet.Enums;
using Packet.Interfaces.Server;

namespace Packet.Server
{
    /// <summary>
    /// Represents a HTTP request parser that will parse requests for HTTP/1.0 and newer.
    /// </summary>
    public class FullHttpRequestParser : HttpRequestParser
    {
        private static Regex _requestLineRegex;
        private static Regex _headerLineRegex;

        /// <summary>
        /// Initializes a new instance of a HTTP request parser that will parse requests for HTTP/1.0 and newer.
        /// </summary>
        /// <param name="successor">The fallback parser to use if parsing is not successful.</param>
        public FullHttpRequestParser(IHttpRequestParser successor) : base(successor)
        {
            _requestLineRegex = _requestLineRegex ?? new Regex("(?i)(\\S+?) (\\S+?) HTTP\\/([0-9]+)\\.([0-9]+)");
            _headerLineRegex = _headerLineRegex ?? new Regex("^(.+?): (.+?)$");
        }

        protected override IHttpRequest AttemptParse(byte[] request)
        {
            using (var memoryStream = new MemoryStream(request))
            using (var streamReader = new StreamReader(memoryStream))
            {
                var requestLine = streamReader.ReadLine();

                // Request line was null, can't continue parsing.
                if (requestLine == null)
                    return null; 

                // Validate request line format.
                var match = _requestLineRegex.Match(requestLine);
                if (!match.Success)
                    return null;

                // Grab method from capturing group.
                var method = HttpMethodConverter.Parse(match.Groups[1].Value);
                if (method == HttpMethod.Unsupported)
                    return null; 

                // Grab URL from capturing group.
                var url = match.Groups[2].Value;

                // Validate URL.
                if (!ValidateUrl(url))
                    return null;

                // Read in headers.
                var headers = new Dictionary<string, string>();
                string headerBuffer;
                while (!string.IsNullOrWhiteSpace(headerBuffer = streamReader.ReadLine()))
                {
                    var headerMatch = _headerLineRegex.Match(headerBuffer);
                    if (!headerMatch.Success)
                        return null;

                    headers.Add(headerMatch.Groups[1].Value, headerMatch.Groups[2].Value);
                }

                using (var bodyStream = new MemoryStream())
                {
                    // Read in rest of body.
                    int bodyBuffer;
                    while ((bodyBuffer = memoryStream.ReadByte()) != -1)
                        bodyStream.WriteByte((byte) bodyBuffer);

                    // Parsing successful.
                    var version = new HttpVersion(int.Parse(match.Groups[3].Value),
                        int.Parse(match.Groups[4].Value));
                    return new FullHttpRequest(method, version)
                    {
                        Headers = headers,
                        Url = url,
                        RequestBody = bodyStream.ToArray()
                    };
                }
            }
        }

        protected override IHttpVersion AttemptGetVersion(string requestLine)
        {
            // Attempt to match request line.
            var match = _requestLineRegex.Match(requestLine);

            // Return version or null if parsing failed.
            return !match.Success ? null 
                : new HttpVersion(int.Parse(match.Groups[3].Value), int.Parse(match.Groups[4].Value));
        }
    }
}
