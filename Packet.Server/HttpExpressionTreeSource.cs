using System.Collections.Generic;
using System.Linq;
using System.Text;

using Crisp.Interfaces.Parsing;
using Crisp.Interfaces.Types;
using Crisp.Types;

using Packet.Enums;
using Packet.Interfaces.Server;

namespace Packet.Server
{
    /// <summary>
    /// Represents an expression tree source created from a HTTP request.
    /// </summary>
    public class HttpExpressionTreeSource : IExpressionTreeSource
    {
        private readonly IHttpRequest _request;

        /// <summary>
        /// Initializes a new instance of an expression tree source created from a HTTP request.
        /// </summary>
        /// <param name="request">The request to create the expression tree from.</param>
        public HttpExpressionTreeSource(IHttpRequest request)
        {
            _request = request;
        }

        /// <summary>
        /// Converts a dictionary of HTTP headers to a serialized Crisp name-value collection.
        /// </summary>
        /// <param name="headers">The header dictionary to convert.</param>
        /// <returns></returns>
        private static ISymbolicExpression TransformHeadersForCrisp(Dictionary<string, string> headers)
        {
            return headers.Select(e => (ISymbolicExpression)new Pair(new StringAtom(e.Key), new StringAtom(e.Value)))
                .ToList()
                .ToProperList();
        }

        public ISymbolicExpression Get()
        {
            // If request was a simple-format request, there won't be headers or a body.
            if (_request.Version.Major < 1)
            {
                return new List<ISymbolicExpression>
                {
                    new StringAtom(_request.Url),
                    new StringAtom(HttpMethodConverter.ToString(HttpMethod.Get)),
                    new Nil(),
                    new Nil()
                }.ToProperList();
            }

            // We now know it's a full request.
            var fullRequest = (FullHttpRequest) _request;
            return new List<ISymbolicExpression>
            {
                new StringAtom(fullRequest.Url),
                new StringAtom(HttpMethodConverter.ToString(fullRequest.Method)),
                new StringAtom(new UTF8Encoding().GetString(fullRequest.RequestBody)),
                TransformHeadersForCrisp(fullRequest.Headers)
            }.ToProperList();
        }
    }
}
