using System;

using Crisp.Core.Evaluation;
using Crisp.Core.Parsing;
using Crisp.Core.Preprocessing;
using Crisp.Visualization;

namespace Crisp
{
    class Program
    {
        static void Main(string[] args)
        {
            // Pre-process input. The pre-processor does the tokenizing.
            var preprocessor = new Preprocessor();
            var tokens = preprocessor.Process("reverse.txt");

            // Create expression tree.
            var parser = new Parser();
            var parsed = parser.CreateExpressionTree(tokens);

            // Create evaluator.
            var evaluator = new Evaluator("native");
            preprocessor.BindExpressions(evaluator); // Load imported bindings from preprocessor.
            var result = evaluator.Evaluate(parsed);

            // Write result to output.
            Console.Write(new LispSerializer().Serialize(result));
        }
    }
}




























































