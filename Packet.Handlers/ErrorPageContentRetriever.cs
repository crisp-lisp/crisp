using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Packet.Interfaces.Configuration;
using Packet.Interfaces.Logging;
using Packet.Interfaces.Server;

namespace Packet.Handlers
{
    public class ErrorPageContentRetriever : IErrorPageContentRetriever
    {
        private readonly IUrlResolver _urlResolver;

        private readonly Encoding _encoding;

        private readonly ILogger _logger;

        private readonly Dictionary<int, string> _statusCodeMappings;

        /// <summary>
        /// Initializes a new instance of a service for retrieving the content of error pages.
        /// </summary>
        /// <param name="packetConfigurationProvider"></param>
        /// <param name="urlResolver"></param>
        /// <param name="encodingProvider"></param>
        /// <param name="logger"></param>
        public ErrorPageContentRetriever(
            IPacketConfigurationProvider packetConfigurationProvider, 
            IUrlResolver urlResolver,
            IEncodingProvider encodingProvider,
            ILogger logger)
        {
            var packetConfiguration = packetConfigurationProvider.Get();

            _urlResolver = urlResolver;
            _encoding = encodingProvider.Get();
            _logger = logger;

            // Initialize status code mapping dictionary.
            _statusCodeMappings = new Dictionary<int, string>
            {
                {
                    400, TryGetPageContent(packetConfiguration.BadRequestErrorPage,
                        Properties.Resources.DefaultErrorPage_400)
                },
                {
                    403, TryGetPageContent(packetConfiguration.ForbiddenErrorPage,
                        Properties.Resources.DefaultErrorPage_403)
                },
                {
                    404, TryGetPageContent(packetConfiguration.NotFoundErrorPage,
                        Properties.Resources.DefaultErrorPage_404)
                },
                {
                    500, TryGetPageContent(packetConfiguration.InternalServerErrorPage,
                        Properties.Resources.DefaultErrorPage_500)
                }
            };
        }

        private string TryGetPageContent(string path, string fallback)
        {
            // If custom error page can't be read, use fallback.
            try
            {
                var errorPagePath = _urlResolver.Resolve(path);
                return File.ReadAllText(errorPagePath);
            }
            catch (Exception ex)
            {
                _logger.WriteError(ex);
                return fallback;
            }
        }

        public string GetErrorPageContent(int errorStatusCode)
        {
            return _statusCodeMappings[errorStatusCode];
        }

        public byte[] GetEncodedErrorPageContent(int errorStatusCode)
        {
            return _encoding.GetBytes(GetErrorPageContent(errorStatusCode));
        }
    }
}
