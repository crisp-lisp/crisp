using System.IO;
using System.Text.RegularExpressions;

using Packet.Interfaces.Server;

namespace Packet.Server
{
    /// <summary>
    /// Represents a HTTP request parser that will parse HTTP/0.9 simple-format requests. 
    /// </summary>
    public class SimpleHttpRequestParser : HttpRequestParser
    {
        private static Regex _requestLineRegex;

        /// <summary>
        /// Initializes a new instance of a HTTP request parser that will parse HTTP/0.9 simple-format requests. 
        /// </summary>
        public SimpleHttpRequestParser()
        {
            // Initialize validation regex.
            _requestLineRegex = _requestLineRegex ?? new Regex("^(?i)GET (\\S*)(?:.*?)\\r?\\n");
        }

        protected override IHttpRequest AttemptParse(byte[] request)
        {
            using (var memoryStream = new MemoryStream(request))
            using (var streamReader = new StreamReader(memoryStream))
            {
                // Validate request line format.
                var match = _requestLineRegex.Match(streamReader.ReadToEnd());
                if (!match.Success)
                {
                    return null; 
                }

                // Grab URL from capturing group.
                var url = match.Groups[1].Value;

                // Validate URL.
                if (!ValidateUrl(url))
                {
                    return null;
                }

                // Parsing successful.
                return new SimpleHttpRequest
                {
                    Url = url
                };
            }
        }

        protected override IHttpVersion AttemptGetVersion(string requestLine)
        {
            return _requestLineRegex.IsMatch(requestLine) ? new HttpVersion(0, 9) : null;
        }
    }
}
