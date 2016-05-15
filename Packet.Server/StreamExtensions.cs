using System;
using System.IO;
using System.Text;

namespace Packet.Server
{
    /// <summary>
    /// Contains extension methods for working with <see cref="Stream"/> instances.
    /// </summary>
    internal static class StreamExtensions
    {
        /// <summary>
        /// Reads a line of text from a <see cref="Stream"/> instance.
        /// </summary>
        /// <param name="stream">The stream to read from.</param>
        /// <param name="preserveLineEnding">Whether or not to preserve the line ending.</param>
        /// <returns></returns>
        public static string ReadLine(this Stream stream, bool preserveLineEnding = false)
        {
            var line = new StringBuilder();

            // Read until newline.
            int buffer;
            while ((buffer = stream.ReadByte()) != '\n')
            {
                switch (buffer)
                {
                    case '\r':
                        line.Append(preserveLineEnding ? "\r" : "");
                        break;
                    case -1:
                        break;
                    default:
                        line.Append(Convert.ToChar(buffer));
                        break;
                }
            }

            return line + (preserveLineEnding ? "\n" : "");
        }
    }
}
