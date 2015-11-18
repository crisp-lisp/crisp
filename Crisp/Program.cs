using System;
using System.IO;

using Crisp.Core.Evaluation;
using Crisp.Core.Parsing;
using Crisp.Core.Preprocessing;
using Crisp.Core.Tokenizing;
using Crisp.Visualization;

namespace Crisp
{
    class Program
    {
        static void Main(string[] args)
        {
            // Pre-process file to get dependencies.
            var preprocessor = new Preprocessor();
            preprocessor.Process("import.txt");

            // Create tokenizer amd tokenize input.
            var tokenizer = TokenizerFactory.GetCrispTokenizer();
            var tokens = tokenizer.Tokenize(File.ReadAllText("import.txt"))
                .RemoveTokens(TokenType.PreprocessorComment, 
                TokenType.PreprocessorImportStatement, 
                TokenType.PreprocessorWhitespace); // Remove preprocessor stuff.

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




























































