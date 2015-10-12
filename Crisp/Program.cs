using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crisp
{
    class Program
    {
        static void Main(string[] args)
        {
            var tokenizer = new Tokenizing.Tokenizer();
            tokenizer.Add(@"[\(]", Tokenizing.TokenType.OpeningParenthesis);
            tokenizer.Add(@"[\)]", Tokenizing.TokenType.ClosingParenthesis);
            tokenizer.Add(@"[0-9]+", Tokenizing.TokenType.Number);
            tokenizer.Add(@"[^\s\(\)]+", Tokenizing.TokenType.Symbol);

            var tokens = tokenizer.Tokenize(File.ReadAllText("input.txt"));
            foreach (var token in tokens)
            {
                Console.WriteLine(token.Type + "\t" + token.Sequence);
            }

            Console.ReadKey(true);
        }
    }
}
