using System.IO;

using Crisp.Evaluation;
using System;
using Crisp.Visualization;
using Crisp.Tokenizing;

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
            var parsed = parser.Parse(tokens);
            Console.WriteLine(new LispSerializer().Serialize(parsed));

            // Create evaluator.
            var evaluator = new Evaluator("native");
            var result = evaluator.Evaluate(parsed);

            Console.WriteLine(new LispSerializer().Serialize(result));
        }
    }
}
