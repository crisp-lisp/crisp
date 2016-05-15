using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using Crisp.Enums;
using Crisp.Interfaces.Shared;
using Crisp.Interfaces.Tokenization;

namespace Crisp.Tokenization
{
    /// <summary>
    /// An implementation of a string tokenizer.
    /// </summary>
    public class Tokenizer : ITokenListSource
    {
        private readonly ISourceCodeProvider _sourceCodeProvider;
        private readonly IList<TokenTemplate> _tokenTemplates;

        /// <summary>
        /// Initializes a new instance of a string tokenizer.
        /// </summary>
        /// <param name="sourceCodeProvider">The service to use to get the path of the source file.</param>
        /// <param name="tokenizerConfigurationProvider">The service to use to configure the tokenizer.</param>
        public Tokenizer(
            ISourceCodeProvider sourceCodeProvider,
            ITokenizerConfigurationProvider tokenizerConfigurationProvider)
        {
            _sourceCodeProvider = sourceCodeProvider;

            // Load token templates.
            _tokenTemplates = new List<TokenTemplate>();
            foreach (var tokenTemplate in tokenizerConfigurationProvider.Get())
            {
                Add(tokenTemplate.Key, tokenTemplate.Value);
            }
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

        /// <summary>
        /// Transforms source code into a list of tokens.
        /// </summary>
        /// <param name="source">The source code to transform.</param>
        /// <returns></returns>
        private IList<IToken> Tokenize(string source)
        {
            var tokens = new List<IToken>();

            // Tokenize input.
            var remaining = source;
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
                            GetColumnPosition(source, remaining), GetLinePosition(source, remaining)));

                        // Trim string from beginning of source.
                        remaining = tokenTemplate.Pattern.Replace(remaining, string.Empty, 1);
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
                    throw new TokenizationException($"Unexpected character '{character}' at line {line} column" +
                                                    $" {column}.", line, column);
                }
            }

            return tokens;
        }

        public IList<IToken> Get()
        {
            return Tokenize(_sourceCodeProvider.Get());
        }
    }
}
