using System.Collections.Generic;
using System.Linq;

using Crisp.Core.Tokenizing;
using Crisp.Core.Types;

namespace Crisp.Core.Parsing
{
    /// <summary>
    /// An implementation of a parser for the Crisp programming language.
    /// </summary>
    public class Parser
    {
        /// <summary>
        /// Returns true if the given token list represents a list expression, otherwise return false.
        /// </summary>
        /// <param name="tokens">The token list to check.</param>
        /// <returns></returns>
        private static bool IsSingleList(IList<Token> tokens)
        {
            if (tokens == null || tokens.Count < 2)
            {
                return false;
            }
            
            var stream = new TokenStream(tokens);
            return stream.ReadExpression().Count == tokens.Count;
        }

        /// <summary>
        /// Removes the first and last token from a list of tokens.
        /// </summary>
        /// <param name="tokens">The token list to operate on.</param>
        /// <returns></returns>
        private static IList<Token> RemoveFirstAndLast(IList<Token> tokens)
        {
            return tokens.Except(new[]
            {
                tokens.First(),
                tokens.Last(),
            }).ToList();
        }

        /// <summary>
        /// Encloses a list of tokens in brackets.
        /// </summary>
        /// <param name="tokens">The token list to enclose.</param>
        /// <returns></returns>
        private static IList<Token> AddBrackets(IList<Token> tokens)
        {
            var brackets = new List<Token>()
            {
                new Token(TokenType.OpeningParenthesis, string.Empty),
                new Token(TokenType.ClosingParenthesis, string.Empty)
            };
            brackets.InsertRange(1, tokens);
            return brackets;
        }

        /// <summary>
        /// Parses a list of tokens into an expression.
        /// </summary>
        /// <param name="tokens">The token list to parse.</param>
        /// <returns></returns>
        private static SymbolicExpression Parse(IList<Token> tokens)
        {
            // If we have a list on our hands.
            if (IsSingleList(tokens))
            {
                /* 
                 * If brackets were in the source code, this is a function application. Otherwise
                 * they're implicit brackets added by the parser and shouldn't be interpreted this way. 
                 */
                var isFunctionApplication = tokens.First().Sequence != string.Empty; 

                var unbracketed = RemoveFirstAndLast(tokens); // Strip outer brackets.

                // If we have nothing inside them, return nil.
                if (!unbracketed.Any())
                {
                    return new Nil();
                }

                var stream = new TokenStream(unbracketed);
                var head = stream.ReadExpression(); // Open a token stream, read head.

                // Check for dotted notation.
                var dotted = stream.Peek();
                if (dotted != null && dotted.Type == TokenType.Dot)
                {
                    stream.Read(); // Get rid of dot.

                    var tail = stream.ReadExpression(); // Read tail expression.

                    // Check for excess.
                    var excess = stream.ReadToEnd();
                    if (excess.Count != 0)
                    {
                        var prob = excess.First();
                        throw new ParsingException("Dotted pair contains too many elements at" +
                                                   $" line {prob.Line} column {prob.Column}.", excess.First());
                    }

                    // Return cons cell.
                    return new Pair(Parse(head), Parse(tail), isFunctionApplication);
                }
                
                // Return cons cell with implicit list as tail.
                return new Pair(Parse(head), Parse(AddBrackets(stream.ReadToEnd())), isFunctionApplication);
            }

            // We should have a single atom here.
            var first = tokens.First();
            if (tokens.Count > 1)
            {
                throw new ParsingException("Freestanding expressions must be inside a list at" +
                                           $" line {first.Line} column {first.Column}.", first);
            }
            
            // Turn token into atom.
            switch (first.Type)
            {
                case TokenType.Symbol:
                    return new SymbolAtom(first.Sequence);
                case TokenType.Numeric:
                    return new NumericAtom(double.Parse(first.Sequence));
                case TokenType.String:
                    return new StringAtom(first.Sequence.Trim('"')); // Remove double quotes from string atoms.
                case TokenType.BooleanTrue:
                    return new BooleanAtom(true);
                case TokenType.BooleanFalse:
                    return new BooleanAtom(false);
                case TokenType.Nil:
                    return new Nil();
                case TokenType.Dot:
                    throw new ParsingException("Encountered unexpected dot notation at" +
                                               $" line {first.Line} column {first.Column}.", first);
                default:
                    throw new ParsingException("Encountered unknown token type at" +
                                               $" line {first.Line} column {first.Column}.", first);
            }
        }

        /// <summary>
        /// Turns a token list into an expression tree.
        /// </summary>
        /// <param name="tokens">The token list to parse.</param>
        /// <returns></returns>
        public SymbolicExpression CreateExpressionTree(IList<Token> tokens)
        {
            return Parse(tokens); // Actually do the parsing.
        }
    }
}
