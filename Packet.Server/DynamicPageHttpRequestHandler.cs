using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using Crisp.Enums;
using Crisp.Interfaces.Serialization;
using Crisp.Interfaces.Types;
using Crisp.IoC;
using Crisp.Types;

using Packet.Enums;
using Packet.Interfaces.Configuration;
using Packet.Interfaces.Server;

namespace Packet.Server
{
    public class DynamicPageHttpRequestHandler : HttpRequestHandler
    {
        private readonly IPacketConfiguration _packetConfiguration;

        private readonly IUrlResolver _urlResolver;

        private readonly ISymbolicExpressionSerializer _symbolicExpressionSerializer;

        public DynamicPageHttpRequestHandler(
            IPacketConfigurationProvider packetConfigurationProvider,
            IUrlResolver urlResolver,
            ISymbolicExpressionSerializer symbolicExpressionSerializer)
        {
            _packetConfiguration = packetConfigurationProvider.Get();
            _urlResolver = urlResolver;
            _symbolicExpressionSerializer = symbolicExpressionSerializer;
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
        /// Converts a dictionary of HTTP headers to a serialized Crisp name-value collection.
        /// </summary>
        /// <param name="headers">The header dictionary to convert.</param>
        /// <returns></returns>
        private string TransformHeadersForCrisp(Dictionary<string, string> headers)
        {
            // We need to serialize to valid Crisp.
            return _symbolicExpressionSerializer.Serialize(
                headers.Select(header => new Pair(new StringAtom(header.Key), new StringAtom(header.Value)))
                    .Cast<ISymbolicExpression>()
                    .ToArray()
                    .ToProperList());
        }

        /// <summary>
        /// Converts a name-value collection of HTTP headers passed back by a Crisp webpage 
        /// </summary>
        /// <param name="headers"></param>
        /// <returns></returns>
        private static Dictionary<string, string> TransformHeadersForPacket(ISymbolicExpression headers)
        {
            if (headers.Type == SymbolicExpressionType.Nil)
            {
                return new Dictionary<string, string>();
            }

            var expanded = headers.AsPair().Expand();
            return expanded.ToDictionary(p => p.AsPair().Head.AsString().Value, p => p.AsPair().Tail.AsString().Value);
        }

        protected override IHttpResponse AttemptHandle(IHttpRequest request)
        {
            var resolvedPath = _urlResolver.Resolve(request.Url); // Resolve URL.

            // If not dynamic page, defer.
            if (!IsInterpretedFileExtension(Path.GetExtension(resolvedPath)))
            {
                return null;
            }

            var fullRequest = request.Version.Major > 0 ? (FullHttpRequest) request : null;

            var runtime = CrispRuntimeFactory.GetCrispRuntime(resolvedPath);

            var verb = fullRequest?.Method ?? HttpMethod.Get;
            var post = fullRequest == null ? string.Empty : Convert.ToBase64String(fullRequest.RequestBody);
            var requestHeaders = fullRequest == null ? "nil" : TransformHeadersForCrisp(fullRequest.Headers);

            var rawArgs = $"(\"{request.Url}\" \"{HttpMethodConverter.ToString(verb)}\" \"{post}\" {requestHeaders})";
            var args = CrispRuntimeFactory.SourceToExpressionTree(rawArgs);

            var result = runtime.Run(args).AsPair().Expand();

            var autoHeaders = new Dictionary<string, string>
            {
                {"Content-Type", result[2].AsString().Value}
            };

            var allHeaders = autoHeaders.Concat(TransformHeadersForPacket(result[3])
                .Where(x => !autoHeaders.ContainsKey(x.Key)))
                .ToDictionary(p => p.Key, p => p.Value);

            return new FullHttpResponse(request.Version)
            {
                StatusCode = Convert.ToInt32(result[1].AsNumeric().Value),
                Content = new UTF8Encoding().GetBytes(result[0].AsString().Value),
                Headers = allHeaders
            };
        }
    }
}
