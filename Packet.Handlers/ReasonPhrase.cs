using System.Collections.Generic;

namespace Packet.Handlers
{
    /// <summary>
    /// Provides a utility class to help with mapping HTTP status codes to their reason phrases.
    /// </summary>
    internal class ReasonPhrase
    {
        /// <summary>
        /// A dictionary mapping HTTP status codes to their reason phrases.
        /// </summary>
        private static readonly Dictionary<int, string> ReasonPhrases = new Dictionary<int, string>
        {
            {200, "OK"},
            {201, "Created"},
            {202, "Accepted"},
            {204, "No Content"},
            {301, "Moved Permanently"},
            {302, "Moved Temporarily"},
            {304, "Not Modified"},
            {400, "Bad Request"},
            {401, "Unauthorized"},
            {403, "Forbidden"},
            {404, "Not Found"},
            {500, "Internal Server Error"},
            {501, "Not Implemented"},
            {502, "Bad Gateway"},
            {503, "Service Unavailable"}
        };

        /// <summary>
        /// Gets the reason phrase for a HTTP status code.
        /// </summary>
        /// <param name="statusCode">The HTTP status code.</param>
        /// <returns></returns>
        public static string Get(int statusCode)
        {
            return ReasonPhrases[statusCode];
        }

        /// <summary>
        /// Gets whether or not there is an official HTTP/1.0 reason phrase for a HTTP status code.
        /// </summary>
        /// <param name="statusCode">The HTTP status code to check.</param>
        /// <returns></returns>
        public static bool Exists(int statusCode)
        {
            return ReasonPhrases.ContainsKey(statusCode);
        }

        /// <summary>
        /// Gets the reason phrase for a HTTP status code or returns a default value if one doesn't exist. 
        /// </summary>
        /// <param name="statusCode">The HTTP status code.</param>
        /// <param name="defaultValue">The default value to return.</param>
        /// <returns></returns>
        public static string TryGet(int statusCode, string defaultValue)
        {
            return Exists(statusCode) ? Get(statusCode) : defaultValue;
        }
    }
}
