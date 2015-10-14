using System.IO;

using Crisp.Evaluation;
using Crisp.Visualization;

namespace Crisp
{
    class Program
    {
        static void Main(string[] args)
        {
            // Create tokeniser amd tokenise input.
            var tokenizer = new Tokenizing.Tokenizer();
            tokenizer.Add(@"[\(]", Tokenizing.TokenType.OpeningParenthesis);
            tokenizer.Add(@"[\)]", Tokenizing.TokenType.ClosingParenthesis);
            tokenizer.Add(@"[0-9]+", Tokenizing.TokenType.Number);
            tokenizer.Add(@"[^\s\(\)]+", Tokenizing.TokenType.Symbol);
            var tokens = tokenizer.Tokenize(File.ReadAllText("input.txt"));

            // Create expression tree.
            var parser = new Parsing.Parser();
            var parsed = parser.Parse(tokens);

            // Create evaluator.
            var evaluator = new Evaluator("native");
            var result = evaluator.Evaluate(parsed);
        }
    }
}
