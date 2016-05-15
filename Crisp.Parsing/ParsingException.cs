using System;

using Crisp.Interfaces.Tokenization;

namespace Crisp.Parsing
{
    /// <summary>
    /// Represents an error encountered during source parsing.
    /// </summary>
    public class ParsingException : Exception
    {
        /// <summary>
        /// Gets the line position in the source at which the parser encountered an error.
        /// </summary>
        public int Line { get; private set; }

        /// <summary>
        /// Gets the column position in the source at which the parser encountered an error.
        /// </summary>
        public int Column { get; private set; }

        /// <summary>
        /// Initializes a new instance of an error encountered during source parsing.
        /// </summary>
        /// <param name="message">The message to show.</param>
        /// <param name="line">The line position in the source at which the parser encountered an error.</param>
        /// <param name="column">The column position in the source at which the parser encountered an error.</param>
        public ParsingException(string message, int line, int column) 
            : base(message)
        {
            Line = line;
            Column = column;
        }

        /// <summary>
        /// Initializes a new instance of an error encountered during source parsing.
        /// </summary>
        /// <param name="message">The message to show.</param>
        /// <param name="token">The token at which the parser encountered an error.</param>
        public ParsingException(string message, IToken token)
            : this(message, token.Line, token.Column)
        {
        }
    }
}
