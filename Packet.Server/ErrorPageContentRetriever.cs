using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Packet.Interfaces.Configuration;
using Packet.Interfaces.Logging;
using Packet.Interfaces.Server;

namespace Packet.Server
{
    public class ErrorPageContentRetriever : IErrorPageContentRetriever
    {
        private readonly IPacketConfiguration _packetConfiguration;
        private readonly IUrlResolver _urlResolver;
        private readonly ILogger _logger;

        public ErrorPageContentRetriever(IPacketConfigurationProvider packetConfigurationProvider, IUrlResolver urlResolver, ILogger logger)
        {
            _packetConfiguration = packetConfigurationProvider.Get();
            _urlResolver = urlResolver;
            _logger = logger;
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

        public string Get400ErrorPageContent()
        {
            return TryGetPageContent(_packetConfiguration.BadRequestErrorPage,
                Properties.Resources.DefaultErrorPage_400);
        }

        public string Get403ErrorPageContent()
        {
            return TryGetPageContent(_packetConfiguration.ForbiddenErrorPage, 
                Properties.Resources.DefaultErrorPage_403);
        }

        public string Get404ErrorPageContent()
        {
            return TryGetPageContent(_packetConfiguration.NotFoundErrorPage,
                Properties.Resources.DefaultErrorPage_404);
        }

        public string Get500ErrorPageContent()
        {
            return TryGetPageContent(_packetConfiguration.InternalServerErrorPage,
                Properties.Resources.DefaultErrorPage_500);
        }
    }
}
