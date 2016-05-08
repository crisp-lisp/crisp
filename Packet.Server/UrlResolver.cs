using System.IO;
using System.Linq;

using Packet.Interfaces.Configuration;
using Packet.Interfaces.Server;

namespace Packet.Server
{
    public class UrlResolver : IUrlResolver
    {
        private readonly IPacketConfiguration _packetConfiguration;

        public UrlResolver(IPacketConfigurationProvider packetConfigurationProvider)
        {
            _packetConfiguration = packetConfigurationProvider.Get();
        }

        public string Resolve(string url)
        {
            // Need to remove slash so it's not considered an absolute path.
            var trimmed = url.TrimStart('/');
            if (trimmed.Contains('?'))
            {
                trimmed = trimmed.Split('?').First(); // Remove query string.
            }

            // Compute real path.
            var path = Path.Combine(_packetConfiguration.WebRoot, trimmed);

            // If real path is a directory.
            if (Directory.Exists(path))
            {
                // Get any configured index pages.
                var files = Directory.GetFiles(path)
                    .Select(Path.GetFileName)
                    .Where(f => _packetConfiguration.DirectoryIndices.Contains(f))
                    .ToArray();
                if (files.Any())
                {
                    // Pass back path to configured index page.
                    path = Path.Combine(path, files.First());
                }
            }

            return path;
        }
    }
}
