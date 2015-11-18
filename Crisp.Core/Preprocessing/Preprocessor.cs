using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Crisp.Core.Evaluation;
using Crisp.Core.Parsing;
using Crisp.Core.Tokenizing;

namespace Crisp.Core.Preprocessing
{
    public class Preprocessor
    {
        public IList<string> LoadedLibraries { get; }

        public void BindExpressions(IEvaluator evaluator)
        {
            foreach (var library in LoadedLibraries)
            {
                var fff = File.ReadAllText(library);
                var tkns = TokenizerFactory.GetCrispTokenizer().Tokenize(fff);
                tkns = tkns.Where(t => t.Type != TokenType.PreprocessorComment)
                    .Where(t => t.Type != TokenType.PreprocessorWhitespace)
                    .Where(t => t.Type != TokenType.PreprocessorImportStatement).ToList();
                var p = new Parser().CreateExpressionTree(tkns);
                var bindings = p.AsPair().Expand();
                foreach (var binding in bindings)
                {
                    evaluator.MutableBind(binding.AsPair().Head.AsSymbol(), binding.AsPair().Tail);
                }
            }
        }

        private bool IsAlreadyImported(string filename)
        {
            return LoadedLibraries.Any(p => p.Equals(filename, StringComparison.InvariantCultureIgnoreCase));
        }

        public void Process(string filename)
        {
            // Check that source file exists.
            if (!File.Exists(filename))
            {
                throw new FileNotFoundException($"File could not be found for preprocessing.", filename);
            }

            // Read source file.
            var fileInfo = new FileInfo(filename);
            var source = File.ReadAllText(filename);
            var tokens = TokenizerFactory.GetCrispTokenizer().Tokenize(source);

            // Remove whitespace and comments.
            var sanitized = tokens.Where(t => t.Type != TokenType.PreprocessorWhitespace)
                .Where(t => t.Type != TokenType.PreprocessorComment).ToList();
            
            // Pop import statements from top of file.
            var importQueue = new Queue<Token>(sanitized);
            while (importQueue.Count > 0 
                && importQueue.Peek().Type == TokenType.PreprocessorImportStatement)
            {
                var import = importQueue.Dequeue();
                var libraryFilename = Regex.Match(import.Sequence, "\"(.+?)\"").Captures[0].Value.Trim('\"');
                var absolutePath = Path.IsPathRooted(libraryFilename)
                    ? libraryFilename
                    : Path.Combine(fileInfo.DirectoryName, libraryFilename);
                if (!IsAlreadyImported(absolutePath))
                {
                    LoadedLibraries.Add(absolutePath);
                    Process(absolutePath);
                }
            }
        }

        public Preprocessor()
        {
            LoadedLibraries = new List<string>();
        }
    }
}
