using System.Collections.Generic;
using System.Linq;

using Crisp.Enums;
using Crisp.Interfaces.Tokenization;

namespace Crisp.Parsing
{
    /// <summary>
    /// Represents a controlled stream of tokens.
    /// </summary>
    public class TokenStream
    {
        private readonly IList<IToken> _tokens;

        /// <summary>
        /// Gets the current position of the stream.
        /// </summary>
        public int Position { get; private set; }

        /// <summary>
        /// Returns the next token in the stream, but does not consume it.
        /// </summary>
        /// <returns></returns>
        public IToken Peek()
        {
            return Position == _tokens.Count ? null : _tokens[Position];
        }

        /// <summary>
        /// Reads the next token from the stream.
        /// </summary>
        /// <returns></returns>
        public IToken Read()
        {
            return Position == _tokens.Count ? null : _tokens[Position++];
        }

        /// <summary>
        /// Reads the next expression (list or atom) from the stream.
        /// </summary>
        /// <returns></returns>
        public IList<IToken> ReadExpression()
        {
            var expression = new List<IToken>();

            var buffer = Read();
            expression.Add(buffer); // Add buffer to result.

            // If we're going to be reading a list.
            if (buffer.Type == TokenType.OpeningParenthesis)
            {
                // Create bracket stack.
                var brackets = new Stack<IToken>();
                brackets.Push(buffer);
                
                // Loop while bracket stack is not empty.
                while (brackets.Any())
                {
                    // Throw if we're at the end of the stream.
                    if ((buffer = Read()) == null)
                    {
                        var bracket = brackets.Peek();
                        throw new ParsingException($"Mismatched opening parenthesis at line {bracket.Line}" +
                                                   $" column {bracket.Column}.", bracket);
                    }

                    // Push or pop brackets.
                    if (buffer.Type == TokenType.OpeningParenthesis)
                    {
                        brackets.Push(buffer);
                    }
                    else if (buffer.Type == TokenType.ClosingParenthesis)
                    {
                        // Too many closing brackets.
                        if (!brackets.Any())
                        {
                            throw new ParsingException($"Mismatched closing parenthesis at line {buffer.Line}" +
                                                       $" column {buffer.Column}.", buffer);
                        }
                        brackets.Pop();
                    }

                    expression.Add(buffer); // Add buffer to result.
                }
            }

            return expression;
        }

        /// <summary>
        /// Reads the rest of this token stream.
        /// </summary>
        /// <returns></returns>
        public IList<IToken> ReadToEnd()
        {
            var remaining = new List<IToken>();
            IToken buffer;
            while ((buffer = Read()) != null)
            {
                remaining.Add(buffer);
            }
            return remaining;
        } 

        /// <summary>
        /// Initializes a new instance of a controlled stream of tokens.
        /// </summary>
        /// <param name="tokens">The list of tokens to stream.</param>
        public TokenStream(IList<IToken> tokens)
        {
            _tokens = tokens;
            Position = 0;
        }
    }
}
