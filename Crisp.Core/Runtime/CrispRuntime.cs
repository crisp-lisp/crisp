using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crisp.Core.Evaluation;
using Crisp.Core.Parsing;
using Crisp.Core.Tokenizing;
using Crisp.Core.Types;

namespace Crisp.Core.Runtime
{
    public class CrispRuntime : ICrispRuntime
    {
        private readonly ITokenizer _tokenizer;
        private readonly IParser _parser;
        private readonly IEvaluator _evaluator;

        public SymbolicExpression EvaluateSource(string source, SymbolicExpression arguments)
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

        public SymbolicExpression EvaluateSource(string source, string arguments)
        {
            // Parse arguments passed in as a string.
            var argumentTokens = _tokenizer.Tokenize($"({arguments})");
            var parsedArgumentList = _parser.CreateExpressionTree(argumentTokens);

            return EvaluateSource(source, parsedArgumentList);
        }

        public SymbolicExpression EvaluateSourceFile(string filepath, SymbolicExpression arguments)
        {
            return EvaluateSource(File.ReadAllText(filepath), arguments);
        }

        public SymbolicExpression EvaluateSourceFile(string filepath, string arguments)
        {
            return EvaluateSource(File.ReadAllText(filepath), arguments);
        }

        public CrispRuntime(ITokenizer tokenizer, IParser parser, IEvaluator evaluator)
        {
            _tokenizer = tokenizer;
            _parser = parser;
            _evaluator = evaluator;
        }
    }
}
