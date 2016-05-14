using System.Collections.Generic;
using System.IO;

using Packet.Interfaces.Server;

namespace Packet.Handlers
{
    /// <summary>
    /// Represents a HTTP handler that serves 404 not found responses for nonexistent resources.
    /// </summary>
    public class NotFoundHttpRequestHandler : HttpRequestHandler
    {
        private readonly IUrlResolver _urlResolver;

        private readonly IErrorPageContentRetriever _errorPageContentRetriever;

        /// <summary>
        /// Initializes a new instance of a HTTP handler that serves 404 not found responses for nonexistent resources.
        /// </summary>
        /// <param name="urlResolver">The URL resolver service.</param>
        /// <param name="errorPageContentRetriever">The error page content retrieval service.</param>
        public NotFoundHttpRequestHandler(
            IUrlResolver urlResolver,
            IErrorPageContentRetriever errorPageContentRetriever)
        {
            _urlResolver = urlResolver;
            _errorPageContentRetriever = errorPageContentRetriever;
        }

        protected override IHttpResponse AttemptHandle(IHttpRequest request)
        {
            var resolvedPath = _urlResolver.Resolve(request.Url);

            // Defer if file exists.
            if (File.Exists(resolvedPath))
            {
                return null;
            }

            return new FullHttpResponse(request.Version)
            {
                StatusCode = 404,
                Headers = new Dictionary<string, string>
                {
                    {"Content-Type", "text/html"}
                },
                Content = _errorPageContentRetriever.GetEncodedErrorPageContent(404)
            };
        }
    }
}
