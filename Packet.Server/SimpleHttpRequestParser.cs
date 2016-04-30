using System;
using System.IO;
using System.Text.RegularExpressions;
using Packet.Interfaces.Server;

namespace Packet.Server
{
    public class SimpleHttpRequestParser : HttpRequestParser
    {
        public SimpleHttpRequestParser(IHttpRequestParser successor) : base(successor)
        {
        }

        protected override IHttpRequest AttemptParse(byte[] request)
        {
            using (var memoryStream = new MemoryStream(request))
            {
                using (var streamReader = new StreamReader(memoryStream))
                {
                    var line = streamReader.ReadToEnd();
                    var regex = new Regex("^GET (\\S*)(?:.*?)\\r?\\n$");

                    if (!regex.IsMatch(line))
                    {
                        return null; // Invalid request line format.
                    }

                    var url = regex.Match(line).Groups[1].Value;

                    Uri validUrl;
                    if (!Uri.TryCreate(url, UriKind.Relative, out validUrl))
                    {
                        return null; // URL is not a valid or not relative.
                    }

                    // Parsing successful.
                    return new SimpleHttpRequest
                    {
                        Url = url
                    };
                }
            }
        }
    }
}
