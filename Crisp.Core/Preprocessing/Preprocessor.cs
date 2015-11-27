using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

using Crisp.Core.Evaluation;
using Crisp.Core.Parsing;
using Crisp.Core.Tokenizing;
using Crisp.Core.Types;

namespace Crisp.Core.Preprocessing
{
    public class Preprocessor : IPreprocessor
    {
        private readonly IDependencyTreeCrawler _dependencyTreeCrawler;
        
        public void BindExpressions(string filepath, IEvaluator evaluator)
        {
            foreach (var library in _dependencyTreeCrawler.Crawl(filepath))
            {
                var source = File.ReadAllText(library);
                
                var rawTokens = TokenizerFactory.GetCrispTokenizer().Tokenize(source);
                rawTokens = rawTokens.RemoveTokens(TokenType.Comment, 
                    TokenType.Whitespace,
                    TokenType.RequireStatement); 
                
                var tokens = new List<Token>
                {
                    new Token(TokenType.OpeningParenthesis, string.Empty),
                    new Token(TokenType.ClosingParenthesis, string.Empty)
                };
                tokens.InsertRange(1, rawTokens);
                
                var parsed = new Parser().CreateExpressionTree(tokens);
                if (parsed.Type != SymbolicExpressionType.Pair)
                {
                    throw new Exception($"Required library '{library}' must contain a list of bindings.");
                }
                
                var bindings = parsed.AsPair().Expand();
                foreach (var binding in bindings)
                {
                    var name = binding.AsPair().Head;
                    if (binding.Type != SymbolicExpressionType.Pair
                        || name.Type != SymbolicExpressionType.Symbol)
                    {
                        throw new Exception(
                            $"Every binding in library '{library}' must take the form of a symbol-expression pair.");
                    }
                    
                    evaluator.MutableBind(binding.AsPair().Head.AsSymbol(), binding.AsPair().Tail);
                }
            }
        }
        
        public Preprocessor(IDependencyTreeCrawler dependencyTreeCrawler)
        {
            _dependencyTreeCrawler = dependencyTreeCrawler;
        }
    }
}
