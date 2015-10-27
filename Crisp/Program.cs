using System;
using System.IO;

using Crisp.Evaluation;
using Crisp.Tokenizing;
using Crisp.Visualization;

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
            var parser = new Parsing.Parser();
            var parsed = parser.CreateExpressionTree(tokens);
            Console.WriteLine(new LispSerializer().Serialize(parsed));

            // Create evaluator.
            var evaluator = new Evaluator("native");
            var result = evaluator.Evaluate(parsed);

            Console.WriteLine(new LispSerializer().Serialize(result));
        }
    }
}
