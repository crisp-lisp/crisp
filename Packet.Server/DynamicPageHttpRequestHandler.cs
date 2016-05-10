using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
    /// <summary>
    /// Represents a HTTP request handler for serving dynamic pages.
    /// </summary>
    public class DynamicPageHttpRequestHandler : HttpRequestHandler
    {
        private readonly IPacketConfiguration _packetConfiguration;

        private readonly IUrlResolver _urlResolver;

        private readonly ISymbolicExpressionSerializer _symbolicExpressionSerializer;

        /// <summary>
        /// Initializes a new instance of a HTTP request handler for serving dynamic pages.
        /// </summary>
        /// <param name="packetConfigurationProvider">The server configuration provider service.</param>
        /// <param name="urlResolver">The URL resolution service.</param>
        /// <param name="symbolicExpressionSerializer">The symbolic expression serialization service.</param>
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
        /// Converts a name-value collection of HTTP headers passed back by a Crisp webpage to a dictionary.
        /// </summary>
        /// <param name="headers">The symbolic expression containingthe name-value collection to convert.</param>
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

            // Create runtime for file.
            var runtime = CrispRuntimeFactory.GetCrispRuntime(resolvedPath);

            // Cast to full request if needed.
            var fullRequest = request.Version.Major > 0 ? (FullHttpRequest)request : null;

            // Extract information from request.
            var verb = fullRequest?.Method ?? HttpMethod.Get;
            var post = fullRequest == null ? string.Empty : Convert.ToBase64String(fullRequest.RequestBody);
            var requestHeaders = fullRequest == null ? "nil" : TransformHeadersForCrisp(fullRequest.Headers);

            // Convert arguments to an expression tree.
            var rawArgs = $"(\"{request.Url}\" \"{HttpMethodConverter.ToString(verb)}\" \"{post}\" {requestHeaders})";
            var args = CrispRuntimeFactory.SourceToExpressionTree(rawArgs);

            // Evaluate webpage.
            IList<ISymbolicExpression> result;
            try
            {
                result = runtime.Run(args).AsPair().Expand();
            }
            catch (Exception)
            {
                // Resolve URL of custom error page.
                var internalServerErrorPagePath = _urlResolver.Resolve(_packetConfiguration.ForbiddenErrorPage);

                // If custom error page not found, use default.
                var errorPageContent = File.Exists(internalServerErrorPagePath)
                    ? File.ReadAllBytes(internalServerErrorPagePath)
                    : new UTF8Encoding().GetBytes(Properties.Resources.DefaultErrorPage_500);

                return new FullHttpResponse(request.Version)
                {
                    StatusCode = 500,
                    Headers = new Dictionary<string, string>
                    {
                        {"Content-Type", "text/html"}
                    },
                    Content = errorPageContent
                };
            }

            // Automatic headers.
            var autoHeaders = new Dictionary<string, string>
            {
                {"Content-Type", result[2].AsString().Value}
            };
            var allHeaders = autoHeaders.Concat(TransformHeadersForPacket(result[3])
                .Where(h => !autoHeaders.ContainsKey(h.Key)))
                .ToDictionary(h => h.Key, h => h.Value);

            // Encode in UTF-8.
            // TODO: Encoding.
            var content = new UTF8Encoding().GetBytes(result[0].AsString().Value);

            // Simple request means simple response.
            if (fullRequest == null)
            {
                return new SimpleHttpResponse(content);
            }

            // Full request means full response.
            return new FullHttpResponse(request.Version)
            {
                StatusCode = Convert.ToInt32(result[1].AsNumeric().Value),
                Content = content,
                Headers = allHeaders
            };
        }
    }
}
