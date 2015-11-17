﻿using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Crisp.Core.Tokenizing
{
    /// <summary>
    /// An implementation of a string tokenizer.
    /// </summary>
    public class Tokenizer : ITokenizer
    {
        private readonly IList<TokenTemplate> _tokenTemplates;

        /// <summary>
        /// Gets or sets whether or not this tokenizer will disregard whitespace.
        /// </summary>
        public bool IgnoreWhitespace { get; set; } = true;

        /// <summary>
        /// Initializes a new instance of a string tokenizer.
        /// </summary>
        public Tokenizer()
        {
            _tokenTemplates = new List<TokenTemplate>();
        }

        /// <summary>
        /// Adds a token template to the tokenizer.
        /// </summary>
        /// <param name="template">The template to add.</param>
        public void Add(TokenTemplate template)
        {
            _tokenTemplates.Add(template);
        }

        /// <summary>
        /// Maps a regular expression to a token type in the tokenizer.
        /// </summary>
        /// <param name="pattern">The regular expression.</param>
        /// <param name="type">The token type.</param>
        public void Add(Regex pattern, TokenType type)
        {
            Add(new TokenTemplate(pattern, type));
        }

        /// <summary>
        /// Maps a regular expression string to a token type in the tokenizer.
        /// </summary>
        /// <param name="pattern">The regular expression string.</param>
        /// <param name="type">The token type.</param>
        public void Add(string pattern, TokenType type)
        {
            Add(new TokenTemplate(pattern, type));
        }

        /// <summary>
        /// Gets the current line position in the source.
        /// </summary>
        /// <param name="source">The complete source.</param>
        /// <param name="remaining">The source remaining to tokenize.</param>
        /// <returns></returns>
        private static int GetLinePosition(string source, string remaining)
        {
            return source.Count(c => c == '\n') - remaining.Count(c => c == '\n') + 1;
        }

        /// <summary>
        /// Gets the current column position in the source.
        /// </summary>
        /// <param name="source">The complete source.</param>
        /// <param name="remaining">The source remaining to tokenize.</param>
        /// <returns></returns>
        private static int GetColumnPosition(string source, string remaining)
        {
            var processed = source.Substring(0, source.Length - remaining.Length);
            return processed.Length - processed.LastIndexOf('\n');
        }

        public IList<Token> Tokenize(string source)
        {
            // TODO: Come back to this. Code smells here.
            var tokens = new List<Token>();

            // Tokenize input.
            var remaining = IgnoreWhitespace ? source.Trim() : source;
            while (!string.IsNullOrEmpty(remaining))
            {
                // Try to match each template against start of input.
                var matches = false;
                foreach (var tokenTemplate in _tokenTemplates)
                {
                    var match = tokenTemplate.Pattern.Match(remaining);
                    if (match.Success && match.Index == 0)
                    {
                        // Add token of matching type.
                        tokens.Add(new Token(tokenTemplate.Type, match.Value, 
                            GetLinePosition(source, remaining), GetColumnPosition(source, remaining)));

                        remaining = tokenTemplate.Pattern.Replace(remaining, string.Empty, 1);
                        if (IgnoreWhitespace)
                        {
                            remaining = remaining.Trim();
                        }
                        matches = true;
                        break;
                    }
                }

                // Unexpected character encountered.
                if (!matches)
                {
                    var line = GetLinePosition(source, remaining);
                    var column = GetColumnPosition(source, remaining);
                    var character = remaining.First();
                    throw new TokenizationException($"Unexpected character '{character}' at line {line} column {column}.", line, column);
                }
            }

            return tokens;
        }
    }
}
