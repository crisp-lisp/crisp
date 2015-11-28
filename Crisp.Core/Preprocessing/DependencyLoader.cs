using System;
using System.Collections.Generic;
using System.IO;

using Crisp.Core.Parsing;
using Crisp.Core.Tokenizing;
using Crisp.Core.Types;

namespace Crisp.Core.Preprocessing
{
    /// <summary>
    /// An implementation of a dependency loader.
    /// </summary>
    public class DependencyLoader : IDependencyLoader
    {
        private readonly ITokenizer _tokenizer;

        private readonly IParser _parser;

        private readonly IDependencyFinder _dependencyFinder;
        
        public Dictionary<SymbolAtom, SymbolicExpression> GetBindings(string filepath)
        {
            // Prepare list of definitions to pass back.
            var definitions = new Dictionary<SymbolAtom, SymbolicExpression>();

            // Find dependency files first.
            var dependencies = _dependencyFinder.FindDependencyFilepaths(filepath);

            // Loop through each dependency file.
            foreach (var library in dependencies)
            {
                // Read and tokenize source file.
                var source = File.ReadAllText(library);
                var tokens = _tokenizer.Tokenize(source);
                
                // Remove comments, whitespace and directives.
                var filter = TokenFilterFactory.GetCommentWhitespaceAndDirectiveFilter();
                tokens = filter.Filter(tokens);
                
                // Wrap whole file in brackets.
                var wrappedTokens = new List<Token>
                {
                    new Token(TokenType.OpeningParenthesis, string.Empty),
                    new Token(TokenType.ClosingParenthesis, string.Empty)
                };
                wrappedTokens.InsertRange(1, tokens);
                
                // Parse wrapped tokens into expression tree.
                var parsed = _parser.CreateExpressionTree(wrappedTokens);
                if (parsed.Type != SymbolicExpressionType.Pair)
                {
                    throw new Exception($"Required library '{library}' must contain a list of bindings.");
                }

                // Pull definitions out of file.
                var bindings = parsed.AsPair().Expand();
                foreach (var binding in bindings)
                {
                    var name = binding.AsPair().Head;
                    if (binding.Type != SymbolicExpressionType.Pair || name.Type != SymbolicExpressionType.Symbol)
                    {
                        throw new Exception(
                            $"Every binding in library '{library}' must take the form of a symbol-expression pair.");
                    }
                    definitions.Add(binding.AsPair().Head.AsSymbol(), binding.AsPair().Tail);
                }
            }

            return definitions;
        }

        /// <summary>
        /// Initializes a new instance of a dependency loader.
        /// </summary>
        /// <param name="tokenizer">The tokenizer to use to tokenize source files.</param>
        /// <param name="parser">The parser to use to parse source files.</param>
        /// <param name="dependencyFinder">The dependency finder to use to locate source file dependencies.</param>
        public DependencyLoader(ITokenizer tokenizer, IParser parser, IDependencyFinder dependencyFinder)
        {
            _tokenizer = tokenizer;
            _parser = parser;
            _dependencyFinder = dependencyFinder;
        }
    }
}
