using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Crisp.Tokenizing
{
    internal class Tokenizer
    {
        private List<TokenTemplate> tokenTemplates;

        public Tokenizer()
        {
            tokenTemplates = new List<TokenTemplate>();
        }

        public void Add(TokenTemplate template)
        {
            tokenTemplates.Add(template);
        }

        public void Add(Regex pattern, TokenType type)
        {
            Add(new TokenTemplate(pattern, type));
        }

        public void Add(string pattern, TokenType type)
        {
            Add(new TokenTemplate(pattern, type));
        }

        public IList<Token> Tokenize(string source)
        {
            var tokens = new List<Token>();

            var remaining = source.Trim();
            while (!string.IsNullOrEmpty(remaining))
            {
                var matches = false;
                foreach (var tokenTemplate in tokenTemplates)
                {
                    var match = tokenTemplate.Pattern.Match(remaining);
                    if (match.Success && match.Index == 0)
                    {
                        tokens.Add(new Token(tokenTemplate.Type, match.Value));
                        remaining = tokenTemplate.Pattern.Replace(remaining, string.Empty, 1).Trim();
                        matches = true;
                        break;
                    }
                }

                if (!matches)
                    throw new TokenizationException("Unexpected input during tokenization.", 
                        source.Length - remaining.Length);
            }

            return tokens;
        }
    }
}
