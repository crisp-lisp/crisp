using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Crisp.Core;
using Crisp.Tokenizing;

namespace Crisp.Parsing
{
    /// <summary>
    /// An implementation of a parser for the Crisp programming language.
    /// </summary>
    internal class Parser
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

        private static IList<Token> RemoveBrackets(IList<Token> tokens)
        {
            return tokens.Except(new[]
            {
                tokens.First(),
                tokens.Last(),
            }).ToList();
        }

        private static IList<Token> AddBrackets(IList<Token> tokens)
        {
            var g = new List<Token>()
            {
                new Token(TokenType.OpeningParenthesis, string.Empty),
                new Token(TokenType.ClosingParenthesis, string.Empty)
            };
            g.InsertRange(1, tokens);
            return g;
        }

        private static bool IsEmptyBrackets(IList<Token> tokens)
        {
            return tokens.Count == 2
                   && tokens.First().Type == TokenType.OpeningParenthesis
                   && tokens.Last().Type == TokenType.ClosingParenthesis;
        }

        private static SymbolicExpression Parse(IList<Token> tokens)
        {
            // If we have a list on our hands.
            if (IsSingleList(tokens))
            {
                var unbracketed = RemoveBrackets(tokens); // Strip outer brackets.

                // If we have nothing inside them, return nil.
                if (!unbracketed.Any())
                {
                    return SymbolAtom.Nil;
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
                    return new Pair(Parse(head), Parse(tail));
                }
                
                // Return cons cell with implicit tail.
                return new Pair(Parse(head), Parse(AddBrackets(stream.ReadToEnd())));
            }

            // We should have a single atom here.
            var first = tokens.First();
            if (tokens.Count > 1)
            {
                throw new ParsingException("Freestanding expressions must be inside a list at" +
                                           $" line {first.Line} column {first.Column}.", first);
            }
            
            switch (first.Type)
            {
                case TokenType.Symbol:
                    return new SymbolAtom(first.Sequence);
                case TokenType.Numeric:
                    return new NumericAtom(double.Parse(first.Sequence));
                case TokenType.String:
                    return new StringAtom(first.Sequence.Trim('"')); // Remove double quotes from string atoms.
                case TokenType.Dot:
                    throw new ParsingException("Encountered unexpected dot notation.", first);
                default:
                    throw new ParsingException("", first);
            }
        }

        public SymbolicExpression CreateExpressionTree(IList<Token> tokens)
        {
            // Use a stack to check brackets match.
            var brackets = new Stack<Token>();
            foreach (var token in tokens)
            {
                if (token.Type == TokenType.OpeningParenthesis)
                {
                    brackets.Push(token);
                }
                else if (token.Type == TokenType.ClosingParenthesis)
                {
                    if (!brackets.Any())
                    {
                        throw new ParsingException($"Mismatched closing parenthesis at line {token.Line}" +
                                                   $" column {token.Column}.", token); // Mismatched closing bracket.
                    }
                    brackets.Pop();
                }
            }

            // Mismatched opening bracket?
            if (brackets.Any())
            {
                var bracket = brackets.Peek();
                throw new ParsingException($"Mismatched opening parenthesis at line {bracket.Line}" +
                                           $" column {bracket.Column}.", bracket);    
            }

            return Parse(tokens); // Actually do the parsing.
        }
    }
}
