using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crisp.Tokenizing;

namespace Crisp.Parsing
{
    internal class TokenStream
    {
        private readonly IList<Token> _tokens;

        public int Position { get; private set; }

        public Token Peek()
        {
            return Position == _tokens.Count ? null : _tokens[Position];
        }

        public Token Read()
        {
            return Position == _tokens.Count ? null : _tokens[Position++];
        }

        public IList<Token> ReadExpression()
        {
            var expression = new List<Token>();

            var buffer = Read();
            expression.Add(buffer);
            if (buffer.Type == TokenType.OpeningParenthesis)
            {
                var brackets = new Stack<Token>();
                brackets.Push(buffer);
                
                while (brackets.Any())
                {
                    buffer = Read();

                    if (buffer == null)
                    {
                        var bracket = brackets.Peek();
                        throw new ParsingException($"Mismatched opening parenthesis at line {bracket.Line}" +
                                                   $" column {bracket.Column}.", bracket);
                    }

                    if (buffer.Type == TokenType.OpeningParenthesis)
                    {
                        brackets.Push(buffer);
                    }
                    else if (buffer.Type == TokenType.ClosingParenthesis)
                    {
                        if (!brackets.Any())
                        {
                            throw new ParsingException($"Mismatched closing parenthesis at line {buffer.Line}" +
                                                       $" column {buffer.Column}.", buffer);
                        }
                        brackets.Pop();
                    }

                    expression.Add(buffer);
                }
            }

            return expression;
        }

        public IList<Token> ReadToEnd()
        {
            var rest = new List<Token>();
            Token buffer = null;
            while ((buffer = Read()) != null)
            {
                rest.Add(buffer);
            }
            return rest;
        } 

        public TokenStream(IList<Token> tokens)
        {
            _tokens = tokens;
            Position = 0;
        }
    }
}
