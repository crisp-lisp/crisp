using System;
using System.IO;

using Crisp.Core.Evaluation;
using Crisp.Core.Parsing;
using Crisp.Core.Tokenizing;

namespace Crisp
{
    class Program
    {
        static void Main(string[] args)
        {
            // Create tokenizer amd tokenize input.
            var tokenizer = TokenizerFactory.GetCrispTokenizer();
            var tokens = tokenizer.Tokenize(File.ReadAllText("input.txt"));

            // Create expression tree.
            var parser = new Parser();
            var parsed = parser.CreateExpressionTree(tokens);

            // Create evaluator.
            var evaluator = new Evaluator("native");
            var result = evaluator.Evaluate(parsed);
        }
    }
}
