using System;

namespace Packet.Interfaces.Logging
{
    /// <summary>
    /// Represents a logger.
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// Writes a line to the log.
        /// </summary>
        /// <param name="msg">The line of text to write to the log.</param>
        void WriteLine(string msg);

        /// <summary>
        /// Writes a string to the log.
        /// </summary>
        /// <param name="msg">The string to write to the log.</param>
        void Write(string msg);

        /// <summary>
        /// Logs an exception.
        /// </summary>
        /// <param name="ex">The exception to log.</param>
        void WriteError(Exception ex);
    }
}
