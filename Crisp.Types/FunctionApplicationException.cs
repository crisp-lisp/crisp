using System;

namespace Crisp.Types
{
    /// <summary>
    /// Represents an error encountered during the application of a function.
    /// </summary>
    public class FunctionApplicationException : Exception
    {
        /// <summary>
        /// Initializes a new instance of an error encountered during the application of a function.
        /// </summary>
        /// <param name="message">The message to show.</param>
        public FunctionApplicationException(string message) : base(message) { }
    }
}
