using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Crisp.Enums;
using Crisp.Interfaces.Parsing;
using Crisp.Interfaces.Types;
using Crisp.Types;

using Packet.Enums;
using Packet.Interfaces.Server;

namespace Packet.Handlers
{
    public class HttpRequestConverter : IHttpRequestConverter
    {
        private readonly Encoding _encoding;

        public HttpRequestConverter(IEncodingProvider encodingProvider)
        {
            _encoding = encodingProvider.Get();
        }

        private static bool IsValidPageResult(ISymbolicExpression result)
        {
            if (result.Type != SymbolicExpressionType.Pair)
            {
                return false;
            }

            var pair = result.AsPair().Expand(); // Expand list.

            // Check types in list.
            return pair[0].Type == SymbolicExpressionType.String
                   && pair[1].Type == SymbolicExpressionType.Numeric
                   && pair[2].Type == SymbolicExpressionType.String
                   && (pair[3].Type == SymbolicExpressionType.Pair || pair[3].Type == SymbolicExpressionType.Nil);
        }

        /// <summary>
        /// Converts a dictionary of HTTP headers to a serialized Crisp name-value collection.
        /// </summary>
        /// <param name="headers">The header dictionary to convert.</param>
        /// <returns></returns>
        private static ISymbolicExpression TransformHeadersToSymbolicExpression(Dictionary<string, string> headers)
        {
            return headers.Select(e => (ISymbolicExpression)new Pair(new StringAtom(e.Key), new StringAtom(e.Value)))
                .ToList()
                .ToProperList();
        }

        /// <summary>
        /// Converts a name-value collection of HTTP headers passed back by a Crisp webpage to a dictionary.
        /// </summary>
        /// <param name="headers">The symbolic expression containingthe name-value collection to convert.</param>
        /// <returns></returns>
        private static Dictionary<string, string> TransformHeadersToDictionary(ISymbolicExpression headers)
        {
            // Nil means empty headers.
            if (headers.Type == SymbolicExpressionType.Nil)
            {
                return new Dictionary<string, string>();
            }

            var expanded = headers.AsPair().Expand();
            return expanded.ToDictionary(p => p.AsPair().Head.AsString().Value, p => p.AsPair().Tail.AsString().Value);
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
                TransformHeadersToSymbolicExpression(fullRequest.Headers)
            }.ToProperList());
        }

        public IHttpResponse ConvertSymbolicExpression(IHttpRequest request, ISymbolicExpression expression)
        {
            IList<ISymbolicExpression> result;

            // Ensure page result was valid.
            if (IsValidPageResult(expression))
            {
                result = expression.AsPair().Expand();
            }
            else
            {
                throw new ApplicationException("Program result was not in a valid format.");
            }

            // Transform headers.
            var headers = TransformHeadersToDictionary(result[3]);
            if (!headers.ContainsKey("Content-Type"))
            {
                headers.Add("Content-Type", result[2].AsString().Value);
            }

            var content = _encoding.GetBytes(result[0].AsString().Value); // Encode content.

            // Simple request means simple response.
            if (request.Version.Major == 0 && request.Version.Minor == 9)
            {
                return new SimpleHttpResponse(content);
            }

            // Full request means full response.
            return new FullHttpResponse(request.Version)
            {
                StatusCode = Convert.ToInt32(result[1].AsNumeric().Value),
                Content = content,
                Headers = headers
            };
        }
    }
}
