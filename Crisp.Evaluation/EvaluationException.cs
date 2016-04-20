using System;

namespace Crisp.Evaluation
{
    /// <summary>
    /// Represents an error encountered during program evaluation.
    /// </summary>
    public class EvaluationException : Exception
    {
        /// <summary>
        /// Initializes a new instance of an error encountered during program evaluation.
        /// </summary>
        /// <param name="message">The message to show.</param>
        public EvaluationException(string message) : base(message)
        {
        }
    }
}
