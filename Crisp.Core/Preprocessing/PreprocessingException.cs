using System;

using Crisp.Core.Tokenizing;

namespace Crisp.Core.Preprocessing
{
    /// <summary>
    /// Represents an error encountered during source preprocessing.
    /// </summary>
    public class PreprocessingException : Exception
    {
        /// <summary>
        /// Gets the line position in the source at which the preprocessor encountered an error.
        /// </summary>
        public int Line { get; private set; }

        /// <summary>
        /// Gets the column position in the source at which the preprocessor encountered an error.
        /// </summary>
        public int Column { get; private set; }

        /// <summary>
        /// Initializes a new instance of an error encountered during source preprocessing.
        /// </summary>
        /// <param name="message">The message to show.</param>
        /// <param name="line">The line position in the source at which the preprocessor encountered an error.</param>
        /// <param name="column">The column position in the source at which the preprocessor encountered an error.</param>
        public PreprocessingException(string message, int line, int column) 
            : base(message)
        {
            Line = line;
            Column = column;
        }

        /// <summary>
        /// Initializes a new instance of an error encountered during source preprocessing.
        /// </summary>
        /// <param name="message">The message to show.</param>
        /// <param name="token">The token at which the preprocessor encountered an error.</param>
        public PreprocessingException(string message, Token token)
            : this(message, token.Line, token.Column)
        {

        }
        /// <summary>
        /// Initializes a new instance of an error encountered during source preprocessing.
        /// </summary>
        /// <param name="message">The message to show.</param>
        public PreprocessingException(string message)
            : this(message, -1, -1)
        { 
        }
    }
}
