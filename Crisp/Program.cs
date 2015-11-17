using System;
using System.IO;

using Crisp.Core.Evaluation;
using Crisp.Core.Parsing;
using Crisp.Core.Tokenizing;
using Crisp.Visualization;

namespace Crisp
{
    class Program
    {
        static void Main(string[] args)
        {
            // Pre-process input.
            var preprocessorTokenizer = TokenizerFactory.GetCrispPreprocessorTokenizer();
            var preprocessorTokens = preprocessorTokenizer.Tokenize(File.ReadAllText("eval_head.txt"));

            // Create tokenizer amd tokenize input.
            var tokenizer = TokenizerFactory.GetCrispTokenizer();
            var tokens = tokenizer.Tokenize(File.ReadAllText("eval_head.txt"));

            // Create expression tree.
            var parser = new Parser();
            var parsed = parser.CreateExpressionTree(tokens);

            // Create evaluator.
            var evaluator = new Evaluator("native");
            var result = evaluator.Evaluate(parsed);

            // Write result to output.
            Console.Write(new LispSerializer().Serialize(result));
        }
    }
}




























































