using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Crisp.Core;

namespace Crisp
{
    class Program
    {
        private static IList<INativeFunction> nativeFunctions;

        /// <summary>
        /// Loads all native function libraries from a directory.
        /// </summary>
        /// <param name="directory"></param>
        /// <returns></returns>
        private static IList<INativeFunction> LoadNativeFunctions(string directory)
        {
            var nativeFunctions = new List<INativeFunction>();

            foreach (var file in Directory.GetFiles(directory, "*.dll"))
            {
                var assembly = Assembly.LoadFrom(file);
                var types = assembly.GetTypes().Where(type => type.IsPublic && !type.IsAbstract); // TODO: Check interface too!

                foreach (var type in types)
                    nativeFunctions.Add((INativeFunction)Activator.CreateInstance(assembly.GetType(type.ToString())));
            }

            return nativeFunctions;
        }

        static void Main(string[] args)
        {
            nativeFunctions = LoadNativeFunctions("native");

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

            var parser = new Parsing.Parser();
            var parsed = parser.Parse(tokens);

            Console.ReadKey(true);
        }
    }
}
