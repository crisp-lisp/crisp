using System.IO;

using Crisp.Core.Evaluation;
using Crisp.Core.Parsing;
using Crisp.Core.Preprocessing;
using Crisp.Core.Tokenizing;
using Crisp.Core.Types;

namespace Crisp.Core.Runtime
{
    public class CrispRuntime : ICrispRuntime
    {
        private readonly ISourceFilePathProvider _sourceFilePathProvider;
        private readonly ITokenizer _tokenizer;
        private readonly IParser _parser;
        private readonly IEvaluator _evaluator;

        private SymbolicExpression EvaluateSource(string source, SymbolicExpression arguments)
        {
            // Remove comments, whitespace, directives.
            var filter = TokenFilterFactory.GetCommentWhitespaceAndDirectiveFilter();
            var tokens = filter.Filter(_tokenizer.Tokenize(source));

            // Create expression tree.
            var parsed = _parser.CreateExpressionTree(tokens);

            // Evaluate program, which should give a function.
            var result = _evaluator.Evaluate(parsed);
            if (result.Type != SymbolicExpressionType.Function)
            {
                throw new RuntimeException("The program must return a lambda for execution.");
            }
            
            // Don't need an evaluator. This is a closure.
            return result.AsFunction().Apply(arguments, null);
        }

        private SymbolicExpression EvaluateSource(string source, string arguments)
        {
            // Parse arguments passed in as a string.
            var filter = TokenFilterFactory.GetCommentAndWhitespaceFilter();
            var argumentTokens = filter.Filter(_tokenizer.Tokenize($"({arguments})"));
            var parsedArgumentList = _parser.CreateExpressionTree(argumentTokens);

            return EvaluateSource(source, parsedArgumentList);
        }

        private SymbolicExpression EvaluateSourceFile(string filepath, SymbolicExpression arguments)
        {
            return EvaluateSource(File.ReadAllText(filepath), arguments);
        }

        private SymbolicExpression EvaluateSourceFile(string filepath, string arguments)
        {
            return EvaluateSource(File.ReadAllText(filepath), arguments);
        }

        public SymbolicExpression Run(string arguments)
        {
            return EvaluateSourceFile(_sourceFilePathProvider.GetPath(), arguments);
        }

        public SymbolicExpression Run(SymbolicExpression arguments)
        {
            return EvaluateSourceFile(_sourceFilePathProvider.GetPath(), arguments);
        }

        public CrispRuntime(
            ISourceFilePathProvider sourceFilePathProvider, 
            ITokenizer tokenizer, 
            IParser parser, 
            IEvaluator evaluator)
        {
            _sourceFilePathProvider = sourceFilePathProvider;
            _tokenizer = tokenizer;
            _parser = parser;
            _evaluator = evaluator;
        }
    }
}
