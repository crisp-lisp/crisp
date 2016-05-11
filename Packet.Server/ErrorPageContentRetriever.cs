using System;
using System.Collections.Generic;
using System.IO;
using System.Web.UI;
using Packet.Interfaces.Configuration;
using Packet.Interfaces.Logging;
using Packet.Interfaces.Server;

namespace Packet.Server
{
    public class ErrorPageContentRetriever : IErrorPageContentRetriever
    {
        private readonly IPacketConfiguration _packetConfiguration;
        private readonly IUrlResolver _urlResolver;
        private readonly IEncodingProvider _encodingProvider;
        private readonly ILogger _logger;

        private readonly Dictionary<int, FileFallbackPair> sss;

        private class FileFallbackPair
        {
            public string FilePath { get; }

            public string Fallback { get; }

            public FileFallbackPair(string filePath, string fallback)
            {
                FilePath = filePath;
                Fallback = fallback;
            }
        }

        public ErrorPageContentRetriever(
            IPacketConfigurationProvider packetConfigurationProvider, 
            IUrlResolver urlResolver,
            IEncodingProvider encodingProvider,
            ILogger logger)
        {
            _packetConfiguration = packetConfigurationProvider.Get();
            _urlResolver = urlResolver;
            _encodingProvider = encodingProvider;
            _logger = logger;
            sss = new Dictionary<int, FileFallbackPair>
            {
                {
                    400, new FileFallbackPair(_packetConfiguration.BadRequestErrorPage,
                        Properties.Resources.DefaultErrorPage_400)
                },
                {
                    403, new FileFallbackPair(_packetConfiguration.ForbiddenErrorPage,
                        Properties.Resources.DefaultErrorPage_403)
                },
                {
                    404, new FileFallbackPair(_packetConfiguration.NotFoundErrorPage,
                        Properties.Resources.DefaultErrorPage_404)
                },
                {
                    500, new FileFallbackPair(_packetConfiguration.InternalServerErrorPage,
                        Properties.Resources.DefaultErrorPage_500)
                }
            };
        }

        private string TryGetPageContent(string path, string fallback)
        {
            // Resolve URL of custom error page.
            var forbiddenPagePath = _urlResolver.Resolve(path);

            // If custom error page can't be read, use fallback.
            try
            {
                return File.ReadAllText(forbiddenPagePath);
            }
            catch (Exception ex)
            {
                _logger.WriteError(ex);
                return fallback;
            }
        }

        public string GetErrorPageContent(int errorStatusCode)
        {
            var gg = sss[errorStatusCode];
            return TryGetPageContent(gg.FilePath, gg.Fallback);
        }

        public byte[] GetEncodedErrorPageContent(int errorStatusCode)
        {
            return _encodingProvider.Get().GetBytes(GetErrorPageContent(errorStatusCode));
        }
    }
}
