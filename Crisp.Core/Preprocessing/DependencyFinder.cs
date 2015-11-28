using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Crisp.Core.Tokenizing;

namespace Crisp.Core.Preprocessing
{
    /// <summary>
    /// An implementation of a dependency finder.
    /// </summary>
    public class DependencyFinder : IDependencyFinder
    {
        private readonly ITokenizer _tokenizer;

        private readonly IRequirePathTransformer _requirePathTransformer;

        private readonly IRequirePathExtractor _requirePathExtractor;
        
        /// <summary>
        /// Recursively searches for all dependencies required by the source file at the given path.
        /// </summary>
        /// <param name="filepath">The path of the source file.</param>
        /// <param name="found">A list of dependencies already found.</param>
        /// <returns></returns> 
        private IList<string> FindDependencyFilepaths(string filepath, IList<string> found)
        {
            // Check file exists.
            if (!File.Exists(filepath))
            {
                throw new FileNotFoundException($"File '{filepath}' could not be found for preprocessing.", filepath);
            }
            
            // Read and tokenize source file.
            var source = File.ReadAllText(filepath);
            var tokens = _tokenizer.Tokenize(source);
            
            // Remove whitespace and comments.
            var filter = TokenFilterFactory.GetCommentAndWhitespaceFilter();
            var sanitized = filter.Filter(tokens);
            
            // Queue up tokens.
            var requireQueue = new Queue<Token>(sanitized);
            while (requireQueue.Count > 0 && requireQueue.Peek().Type == TokenType.RequireStatement)
            {
                // Pop require statement off queue.
                var require = requireQueue.Dequeue();
                var rawFilename = _requirePathExtractor.Extract(require.Sequence); // Extract filename from token sequence.
                var libraryFilename = _requirePathTransformer.Transform(rawFilename); // Transform path according to special rules.
                
                // Record filename if we haven't found it yet.
                if (!found.Any(l => l.Equals(libraryFilename, StringComparison.InvariantCultureIgnoreCase))) 
                {
                    found.Add(libraryFilename);
                    FindDependencyFilepaths(libraryFilename, found);
                }
            }
            
            return found;
        }

        public IList<string> FindDependencyFilepaths(string filepath)
        {
            return FindDependencyFilepaths(filepath, new List<string>());
        }

        /// <summary>
        /// Initializes a new instance of a dependency finder.
        /// </summary>
        /// <param name="tokenizer">The tokenizer to use to tokenize source files.</param>
        /// <param name="requirePathExtractor">The require path extractor to use to extract require paths from token sequences.</param>
        /// <param name="requirePathTransformer">The require path transformer to use to transform require paths for loading.</param>
        public DependencyFinder(ITokenizer tokenizer, IRequirePathExtractor requirePathExtractor,
            IRequirePathTransformer requirePathTransformer)
        {
            _tokenizer = tokenizer;
            _requirePathExtractor = requirePathExtractor;
            _requirePathTransformer = requirePathTransformer;
        }
    }
}
