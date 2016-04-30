using System.IO;
using System.Text.RegularExpressions;

using Packet.Interfaces.Server;

namespace Packet.Server
{
    /// <summary>
    /// Represents a HTTP request parser that will parse HTTP/0.9 simple-format requests. 
    /// </summary>
    public class HttpZeroPointNineRequestParser : HttpRequestParser
    {
        private static Regex _validationRegex;

        /// <summary>
        /// Initializes a new instance of a HTTP request parser that will parse HTTP/0.9 simple-format requests. 
        /// </summary>
        /// <param name="successor">The fallback parser to use if parsing is not successful.</param>
        public HttpZeroPointNineRequestParser(IHttpRequestParser successor) : base(successor)
        {
            // Initialize validation regex.
            _validationRegex = _validationRegex ?? new Regex("^(?i)GET (\\S*)(?:.*?)\\r?\\n");
        }

        protected override IHttpRequest AttemptParse(byte[] request)
        {
            using (var memoryStream = new MemoryStream(request))
            {
                using (var streamReader = new StreamReader(memoryStream))
                {
                    // Validate request line format.
                    var match = _validationRegex.Match(streamReader.ReadToEnd());
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
                    return new HttpZeroPointNineRequest
                    {
                        Url = url
                    };
                }
            }
        }
    }
}
