using System;
using System.Collections.Generic;
using System.Linq;

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
        /// Gets whether or not a token list is enclosed in brackets.
        /// </summary>
        /// <param name="tokens">The token list to check.</param>
        /// <returns></returns>
        private bool IsBracketed(IList<Token> tokens)
        {
            return tokens.First().Type == TokenType.OpeningParenthesis
                && tokens.Last().Type == TokenType.ClosingParenthesis;
        }

        /// <summary>
        /// Removes a set of brackets from around a list of tokens.
        /// </summary>
        /// <param name="tokens">The token list to operate on.</param>
        /// <returns></returns>
        private IList<Token> Unbracket(IList<Token> tokens)
        {
            if (!IsBracketed(tokens))
                throw new ParsingException("Tried to unbracket a non-bracketed token list.");

            return tokens.Except(new[]
            {
                tokens.First(),
                tokens.Last(),
            }).ToList();
        }
                
        /// <summary>
        /// Wraps a list of tokens in a set of brackets.
        /// </summary>
        /// <param name="tokens">The token list to operate on.</param>
        /// <returns></returns>
        private IList<Token> Bracket(IList<Token> tokens)
        {
            var result = new List<Token>()
            {
                new Token(TokenType.OpeningParenthesis, string.Empty),
                new Token(TokenType.ClosingParenthesis, string.Empty), 
            };
            result.InsertRange(1, tokens);

            return result;
        }

        /// <summary>
        /// Reads a list from the beginning of the given list of tokens.
        /// </summary>
        /// <param name="tokens">The token list to operate on.</param>
        /// <returns></returns>
        private IList<Token> ReadList(IList<Token> tokens)
        {
            // TODO: Messy logic, clean up.
            if (tokens.First().Type != TokenType.OpeningParenthesis)
                throw new ParsingException("Tried to read list, but no list found at beginning of token list.");

            var result = new List<Token>();
            
            var level = 0; 
            for (int i = 0; i == 0 || level > 0; i++)
            {
                if (i == tokens.Count())
                    throw new ParsingException("Mismatched parenthesis.");
                
                if (tokens[i].Type == TokenType.OpeningParenthesis)
                    level++;
                else if (tokens[i].Type == TokenType.ClosingParenthesis)
                    level--;

                result.Add(tokens[i]);
            }

            return result;
        }

        /// <summary>
        /// Gets the first expression from a list of tokens.
        /// </summary>
        /// <param name="tokens">The token list to operate on.</param>
        /// <returns></returns>
        private IList<Token> Head(IList<Token> tokens)
        {
            // Is head a list?
            if (tokens.First().Type == TokenType.OpeningParenthesis)
                return ReadList(tokens); 
            else
                return new List<Token>()
                {
                    tokens.First(),
                };
        }

        /// <summary>
        /// Gets every token in a list of tokens apart from the first expression.
        /// </summary>
        /// <param name="tokens">The list of tokens to parse.</param>
        /// <returns></returns>
        private IList<Token> Tail(IList<Token> tokens)
        {
            // Tail is always a list.
            return Bracket(tokens.Except(Head(tokens)).ToList()); 
        }

        /// <summary>
        /// Parses a list of tokens into an expresion tree.
        /// </summary>
        /// <param name="tokens">The list of tokens to parse.</param>
        /// <returns></returns>
        public SymbolicExpression Parse(IList<Token> tokens)
        {
            // If we have an atomic token.
            if (!IsBracketed(tokens))
            {
                var first = tokens.First();
                switch (first.Type)
                {
                    case TokenType.Symbol:
                        return new SymbolicExpression(first.Sequence);
                    case TokenType.Integer:
                        return new SymbolicExpression(int.Parse(first.Sequence));
                    case TokenType.Real:
                        return new SymbolicExpression(double.Parse(first.Sequence));
                    case TokenType.String:
                        return new SymbolicExpression(first.Sequence.Trim('"'), true); // Remove double quotes from string atoms.
                }
            }

            // If we have a list-type token, remove brackets.
            var unbracketed = Unbracket(tokens);
            if (unbracketed.Count() == 0)
                return null;
            
            // Recurse into list.
            return new SymbolicExpression(
                Parse(Head(unbracketed)), 
                Parse(Tail(unbracketed)));
        }
    }
}
