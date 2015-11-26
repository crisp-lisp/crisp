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
    /// <summary>
    /// An implementation of a preprocessor for the Crisp language.
    /// </summary>
    public class Preprocessor : IPreprocessor
    {
        /// <summary>
        /// Gets a list of the filepaths of currently loaded (required) libraries.
        /// </summary>
        public IList<string> LoadedLibraries { get; }
        
        /// <summary>
        /// Gets the directory path that the currently running interpreter executable is located at.
        /// </summary>
        public string InterpreterDirectory { get; private set; }

        /// <summary>
        /// Returns true if the library with the given filename has already been required.
        /// </summary>
        /// <param name="filename">The filename to check.</param>
        /// <returns></returns>
        private bool IsAlreadyRequired(string filename)
        {
            return LoadedLibraries.Any(p => p.Equals(filename, StringComparison.InvariantCultureIgnoreCase));
        }

        /// <summary>
        /// Extracts the filename from an require statement token.
        /// </summary>
        /// <param name="token">The require statement token to extract the filename from.</param>
        /// <returns></returns>
        private static string ExtractFilename(Token token)
        {
            if (token.Type != TokenType.RequireStatement)
            {
                throw new PreprocessingException($"Could not extract filename from token of type '{token.Type}'.");
            }
            return Regex.Match(token.Sequence, "\"(.+?)\"").Captures[0].Value.Trim('\"');
        }

        public void BindExpressions(IEvaluator evaluator)
        {
            foreach (var library in LoadedLibraries)
            {
                var source = File.ReadAllText(library);

                // Remove comments, whitespace and requires (we have crawled them already).
                var rawTokens = TokenizerFactory.GetCrispTokenizer().Tokenize(source);
                rawTokens = rawTokens.RemoveTokens(TokenType.Comment, 
                    TokenType.Whitespace,
                    TokenType.RequireStatement); 

                // Enclose entire file in brackets.
                var tokens = new List<Token>
                {
                    new Token(TokenType.OpeningParenthesis, string.Empty),
                    new Token(TokenType.ClosingParenthesis, string.Empty)
                };
                tokens.InsertRange(1, rawTokens);

                // Parse tokens and check we got one list of bindings back.
                var parsed = new Parser().CreateExpressionTree(tokens);
                if (parsed.Type != SymbolicExpressionType.Pair)
                {
                    throw new PreprocessingException($"Required library '{library}' must contain a list of bindings.");
                }

                // Go through each binding.
                var bindings = parsed.AsPair().Expand();
                foreach (var binding in bindings)
                {
                    // Binding must take the form of a pair binding a symbol to an expression.
                    var name = binding.AsPair().Head;
                    if (binding.Type != SymbolicExpressionType.Pair
                        || name.Type != SymbolicExpressionType.Symbol)
                    {
                        throw new PreprocessingException(
                            $"Every binding in library '{library}' must take the form of a symbol-expression pair.");
                    }

                    // Add binding to evaluator.
                    evaluator.MutableBind(binding.AsPair().Head.AsSymbol(), binding.AsPair().Tail);
                }
            }
        }

        public IList<Token> Process(string filename)
        {
            // Check that source file exists.
            if (!File.Exists(filename))
            {
                throw new FileNotFoundException($"File '{filename}' could not be found for preprocessing.", filename);
            }

            // Read source file.
            var source = File.ReadAllText(filename);
            var tokens = TokenizerFactory.GetCrispTokenizer().Tokenize(source);
            
            // Remove whitespace and comments.
            var sanitized = tokens.RemoveTokens(TokenType.Whitespace,
                TokenType.Comment);
            
            // Process requires.
            var requireQueue = new Queue<Token>(sanitized);
            while (requireQueue.Count > 0 
                && requireQueue.Peek().Type == TokenType.RequireStatement)
            {
                // Pop require statement from top of file, extract filename.
                var require = requireQueue.Dequeue();
                var libraryFilename = ExtractFilename(require);
                if (libraryFilename.StartsWith("~/"))
                {
                    libraryFilename = libraryFilename.Trim('~');
                    libraryFilename = InterpreterDirectory.TrimEnd('\'') + libraryFilename;
                }

                // Use absolute or relative path.
                var fileInfo = new FileInfo(filename);
                var absolutePath = Path.IsPathRooted(libraryFilename)
                    ? libraryFilename
                    : Path.Combine(fileInfo.DirectoryName ?? string.Empty, libraryFilename);

                // Check that this library isn't already loaded.
                if (!IsAlreadyRequired(absolutePath)) 
                {
                    LoadedLibraries.Add(absolutePath);
                    Process(absolutePath);
                }
            }

            // Return sanitized token list.
            return requireQueue.ToList(); 
        }

        /// <summary>
        /// Initializes a new instance of a preprocessor for the Crisp language.
        /// </summary>
        public Preprocessor(string interpreterDirectory)
        {
            LoadedLibraries = new List<string>();
            InterpreterDirectory = interpreterDirectory.TrimEnd('\'');
        }
    }
}
