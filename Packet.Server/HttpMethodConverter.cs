using System;
using System.Linq;

using Packet.Enums;

namespace Packet.Server
{
    /// <summary>
    /// A static helper class to convert from HTTP method strings to their <see cref="HttpMethod"/> equivalents.
    /// </summary>
    public static class HttpMethodConverter
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
    }
}
