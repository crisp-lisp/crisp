using System;

namespace Crisp.Core
{
    /// <summary>
    /// Represents an error encountered during program evaluation.
    /// </summary>
    public class RuntimeException : Exception
    {
        /// <summary>
        /// Initializes a new instance of an error encountered during program evaluation.
        /// </summary>
        /// <param name="message">The message to show.</param>
        public RuntimeException(string message) : base(message) { }
    }
}
