using System;
using System.Linq;

using Packet.Enums;

namespace Packet.Handlers
{
    /// <summary>
    /// A static helper class to convert from HTTP method strings to their <see cref="HttpMethod"/> equivalents.
    /// </summary>
    internal static class HttpMethodHelper
    {
        /// <summary>
        /// Converts from a HTTP method string to its equivalent enum member.
        /// </summary>
        /// <param name="method">The method string to convert.</param>
        /// <returns></returns>
        public static HttpMethod Parse(string method)
        {
            // Case-insensitive check against names.
            var name = Enum.GetNames(typeof (HttpMethod))
                .FirstOrDefault(n => n.Equals(method, StringComparison.OrdinalIgnoreCase));

            return name == null ? HttpMethod.Unsupported : (HttpMethod) Enum.Parse(typeof (HttpMethod), name);
        }

        /// <summary>
        /// Converts from a HTTP method enum value to its equivalent string.
        /// </summary>
        /// <param name="method">The method enum value to convert.</param>
        /// <returns></returns>
        public static string ToString(HttpMethod method)
        {
            return Enum.GetName(typeof (HttpMethod), method)?.ToUpper(); // Make it uppercase.
        }
    }
}
