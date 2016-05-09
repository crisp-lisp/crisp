using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Crisp.IoC;

using Packet.Enums;
using Packet.Interfaces.Configuration;
using Packet.Interfaces.Server;

namespace Packet.Server
{
    public class DynamicPageHttpRequestHandler : HttpRequestHandler
    {
        private readonly IPacketConfiguration _packetConfiguration;

        private readonly IUrlResolver _urlResolver;
        
        public DynamicPageHttpRequestHandler(
            IPacketConfigurationProvider packetConfigurationProvider,
            IUrlResolver urlResolver)
        {
            _packetConfiguration = packetConfigurationProvider.Get();
            _urlResolver = urlResolver;
        }

        /// <summary>
        /// Returns true if the given file extension if configured to be interpreted.
        /// </summary>
        /// <param name="extension">The file extension to check, including the leading dot.</param>
        /// <returns></returns>
        private bool IsInterpretedFileExtension(string extension)
        {
            return _packetConfiguration.CrispFileExtensions.Contains(extension);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dictionary"></param>
        /// <returns></returns>
        private static string SerializeDictionary(Dictionary<string, string> dictionary)
        {
            var sb = new StringBuilder();

            // Serialize entries.
            foreach (var entry in dictionary)
            {
                sb.Append($"((\"{entry.Key}\" . \"{entry.Value}\") . ");
            }

            // Terminate with nil.
            sb.Append("nil");

            // Close all parentheses. 
            for (var i = 0; i < dictionary.Count; i++)
            {
                sb.Append(")");
            }

            return sb.ToString();
        }

        protected override IHttpResponse AttemptHandle(IHttpRequest request)
        {
            var resolvedPath = _urlResolver.Resolve(request.Url); // Resolve URL.

            // If not dynamic page, defer.
            if (!IsInterpretedFileExtension(Path.GetExtension(resolvedPath)))
            {
                return null;
            }

            var runtime = CrispRuntimeFactory.GetCrispRuntime(resolvedPath);
            var verb = request.Version.Major > 0 ? ((FullHttpRequest) request).Method : HttpMethod.Get;
            var post = request.Version.Major > 0 ? Convert.ToBase64String(((FullHttpRequest)request).RequestBody) : "";
            var hh = request.Version.Major > 0 ? SerializeDictionary(((FullHttpRequest) request).Headers) : "nil";
            var s =
                CrispRuntimeFactory.SourceToExpressionTree(
                    $"(\"{request.Url}\" \"{HttpMethodConverter.ToString(verb)}\" \"{post}\" {hh})");
            var result = runtime.Run(s);
            throw new NotImplementedException();
            //url verb post headers)

        }
    }
}
