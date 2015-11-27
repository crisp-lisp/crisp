using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Crisp.Core.Tokenizing;

namespace Crisp.Core.Preprocessing
{
    public class DependencyTreeCrawler : IDependencyTreeCrawler
    {
        private readonly ITokenizer _tokenizer;

        private readonly IRequirePathTransformer _requirePathTransformer;

        private readonly IRequirePathExtractor _requirePathExtractor;

        public IList<string> Crawl(string filepath, IList<string> loaded = null)
        {
            loaded = loaded ?? new List<string>();
            
            if (!File.Exists(filepath))
            {
                throw new FileNotFoundException($"File '{filepath}' could not be found for preprocessing.", filepath);
            }
            
            var source = File.ReadAllText(filepath);
            var tokens = _tokenizer.Tokenize(source);
            
            var sanitized = tokens.RemoveTokens(TokenType.Whitespace, TokenType.Comment);
            
            var requireQueue = new Queue<Token>(sanitized);
            while (requireQueue.Count > 0 && requireQueue.Peek().Type == TokenType.RequireStatement)
            {
                var require = requireQueue.Dequeue();
                var rawFilename = _requirePathExtractor.Extract(require.Sequence);
                var libraryFilename = _requirePathTransformer.Transform(rawFilename);
                
                if (!loaded.Any(l => l.Equals(libraryFilename, StringComparison.InvariantCultureIgnoreCase))) 
                {
                    loaded.Add(libraryFilename);
                    return Crawl(libraryFilename, loaded);
                }
            }
            
            return loaded;
        }
        
        public DependencyTreeCrawler(ITokenizer tokenizer, IRequirePathExtractor requirePathExtractor, IRequirePathTransformer requirePathTransformer)
        {
            _tokenizer = tokenizer;
            _requirePathExtractor = requirePathExtractor;
            _requirePathTransformer = requirePathTransformer;
        }
    }
}
