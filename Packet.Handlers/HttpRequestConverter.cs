using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crisp.Interfaces.Parsing;
using Crisp.Interfaces.Types;
using Crisp.Types;

using Packet.Enums;
using Packet.Interfaces.Server;

namespace Packet.Handlers
{
    /// <summary>
    /// Represents an expression tree source created from a HTTP request.
    /// </summary>
    public class HttpRequestConverter : IHttpRequestConverter
    {
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

        public IExpressionTreeSource ConvertHttpRequest(IHttpRequest request)
        {
            // If request was a simple-format request, there won't be headers or a body.
            if (request.Version.Major < 1)
            {
                return new ExpressionTreeSource(new List<ISymbolicExpression>
                {
                    new StringAtom(request.Url),
                    new StringAtom(HttpMethodHelper.ToString(HttpMethod.Get)),
                    new Nil(),
                    new Nil()
                }.ToProperList());
            }

            // We now know it's a full request.
            var fullRequest = (IFullHttpRequest) request;
            return new ExpressionTreeSource(new List<ISymbolicExpression>
            {
                new StringAtom(fullRequest.Url),
                new StringAtom(HttpMethodHelper.ToString(fullRequest.Method)),
                new StringAtom(new UTF8Encoding().GetString(fullRequest.RequestBody)),
                TransformHeadersForCrisp(fullRequest.Headers)
            }.ToProperList());
        }
    }
}
